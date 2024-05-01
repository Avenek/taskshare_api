using API_project_system.Entities;
using API_project_system.Enums;
using API_project_system.Exceptions;
using API_project_system.Logger;
using API_project_system.ModelsDto;
using API_project_system.ModelsDto.RepositoryDto;
using API_project_system.Specifications;
using API_project_system.Specifications.CourseSpecifications;
using API_project_system.Specifications.RepositorySpecification;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace API_project_system.Services
{
    public interface ICourseService
    {
        public PageResults<CourseDto> GetAll(GetAllQuery queryParameters);
        public PageResults<CourseDto> GetAllEnrolledByUser(GetAllQuery queryParameters);
        public PageResults<CourseDto> GetAllPendingByUser(GetAllQuery queryParameters);
        public PageResults<CourseDto> GetAllByUser(GetAllQuery queryParameters);
        public CourseDto GetById(int courseId);
    }
    public class CourseService : ICourseService
    {
        public IUnitOfWork UnitOfWork { get; }
        private readonly IMapper mapper;
        private readonly UserActionLogger logger;
        private readonly IPaginationService queryParametersService;
        private readonly IUserContextService userContextService;

        public CourseService(IUnitOfWork unitOfWork, IMapper mapper, UserActionLogger logger, 
            IPaginationService queryParametersService, IUserContextService userContextService)
        {
            UnitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
            this.queryParametersService = queryParametersService;
            this.userContextService = userContextService;
        }

        public PageResults<CourseDto> GetAll(GetAllQuery queryParameters)
        {
            var spec = new CoursesWithOwnerSpecification(queryParameters.SearchPhrase);
            var courseQuery = UnitOfWork.Courses.GetBySpecification(spec);
            var result = queryParametersService.PreparePaginationResults<CourseDto, Course>(queryParameters, courseQuery, mapper);
            result.Items.Select(f => f.ApprovalStatus = EApprovalStatus.None);

            return result;
        }

        public PageResults<CourseDto> GetAllEnrolledByUser(GetAllQuery queryParameters)
        {
            var userId = userContextService.GetUserId;
            var spec = new EnrolledCoursesByUserIdWithOwnerSpecification(userId, queryParameters.SearchPhrase);
            var courseQuery = UnitOfWork.CoursesEnrolledUsers.GetBySpecification(spec).Select(f => f.Course);
            var result = queryParametersService.PreparePaginationResults<CourseDto, Course>(queryParameters, courseQuery, mapper);
            result.Items.Select(f => f.ApprovalStatus = EApprovalStatus.Confirmed);

            return result;
        }

        public PageResults<CourseDto> GetAllPendingByUser(GetAllQuery queryParameters)
        {
            var userId = userContextService.GetUserId;
            var spec = new PendingCoursesByUserIdWithOwnerSpecification(userId, queryParameters.SearchPhrase);
            var courseQuery = UnitOfWork.CoursesPendingUsers.GetBySpecification(spec).Select(f => f.Course);
            var result = queryParametersService.PreparePaginationResults<CourseDto, Course>(queryParameters, courseQuery, mapper);
            result.Items.Select(f => f.ApprovalStatus = EApprovalStatus.NeedsConfirmation);

            return result;
        }

        public PageResults<CourseDto> GetAllByUser(GetAllQuery queryParameters)
        {
            var resultPending = GetAllPendingByUser(queryParameters);
            var resultEnrolled = GetAllEnrolledByUser(queryParameters);

            var concatResult = new PageResults<CourseDto>(resultPending.Items.Concat(resultEnrolled.Items).ToList(), 
                resultPending.TotalItemsCount + resultEnrolled.TotalItemsCount,
                queryParameters.PageSize,
                queryParameters.PageNumber);

            return concatResult;
        }

        public CourseDto GetById(int courseId)
        {
            var userId = userContextService.GetUserId;
            Course course = GetCourseIfUserBelongsTo(userId, courseId);
            var courseDto = mapper.Map<CourseDto>(course);
            courseDto.ApprovalStatus = EApprovalStatus.Confirmed;
            return courseDto;
        }

        private Course GetCourseIfUserBelongsTo(int userId, int courseId)
        {
            var course = UnitOfWork.Courses.GetById(courseId);
            if (course is null)
            {
                throw new NotFoundException("That entity doesn't exist.");
            }
            var enrolledCourses = UnitOfWork.CoursesEnrolledUsers.GetAllByUser(userId);
            if (course.UserId != userId && !enrolledCourses.Exists(f => f.CourseId == courseId))
            {
                throw new ForbidException("Cannot access to this course.");
            }

            return course;
        }
    }
}
