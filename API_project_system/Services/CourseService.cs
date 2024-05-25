using API_project_system.Authorization;
using API_project_system.Entities;
using API_project_system.Enums;
using API_project_system.Exceptions;
using API_project_system.Logger;
using API_project_system.ModelsDto;
using API_project_system.ModelsDto.CourseDto;
using API_project_system.Specifications.CourseSpecifications;
using API_project_system.Specifications.RepositorySpecification;
using API_project_system.Transactions;
using API_project_system.Transactions.Courses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace API_project_system.Services
{
    public interface ICourseService
    {
        public IUnitOfWork UnitOfWork { get; }
        public PageResults<CourseDto> GetAll(GetAllQuery queryParameters);
        public PageResults<CourseDto> GetAllEnrolledByUser(GetAllQuery queryParameters);
        public PageResults<CourseDto> GetAllPendingByUser(GetAllQuery queryParameters);
        public PageResults<CourseDto> GetAllByUser(GetAllQuery queryParameters);
        public CourseMembersDto GetAllMembersWithStatus(int courseId);
        public CourseDto GetById(int courseId);
        public void JoinCourse(int courseId);
        public void AcceptMember(CourseMemberDto courseMemberDto);
        public void RemoveMember(CourseMemberDto courseMemberDto);
        public Course CreateCourse(AddCourseDto addCourseDto);
        public void DeleteCourse(int courseId);
        public void UpdateCourse(int courseId, UpdateCourseDto updateCourseDto);
    }
    public class CourseService : ICourseService
    {
        public IUnitOfWork UnitOfWork { get; }
        private readonly IMapper mapper;
        private readonly UserActionLogger logger;
        private readonly IPaginationService queryParametersService;
        private readonly IUserContextService userContextService;
        private readonly IAuthorizationService authorizationService;

        public CourseService(IUnitOfWork unitOfWork, IMapper mapper, UserActionLogger logger,
            IPaginationService queryParametersService, IUserContextService userContextService, IAuthorizationService authorizationService)
        {
            UnitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
            this.queryParametersService = queryParametersService;
            this.userContextService = userContextService;
            this.authorizationService = authorizationService;
        }

        public PageResults<CourseDto> GetAll(GetAllQuery queryParameters)
        {
            var userId = userContextService.GetUserId;
            var spec = new CoursesWithOwnerSpecification(queryParameters.SearchPhrase);
            var courseQuery = UnitOfWork.Courses.GetBySpecification(spec);
            var enrolledUserIds =  courseQuery
            .SelectMany(course => course.EnrolledUsers.Select(user => new { course.Id, UserId = user.Id })
                .GroupBy(x => x.Id)).ToDictionary(g => g.Key, g => g.Select(x => x.UserId).ToList());
            var pendingUserIds = courseQuery
            .SelectMany(course => course.PendingUsers.Select(user => new { course.Id, UserId = user.Id })
                .GroupBy(x => x.Id))
            .ToDictionary(g => g.Key, g => g.Select(x => x.UserId).ToList());
            var result = queryParametersService.PreparePaginationResults<CourseDto, Course>(queryParameters, courseQuery, mapper);

            foreach (var item in result.Items)
            {
                if (enrolledUserIds[item.Id].Contains(userId))
                {
                    item.ApprovalStatus = EApprovalStatus.Confirmed;
                }
                else if (pendingUserIds[item.Id].Contains(userId))
                {
                    item.ApprovalStatus = EApprovalStatus.NeedsConfirmation;
                }
                else
                {
                    item.ApprovalStatus = EApprovalStatus.None;
                }
            }

            return result;
        }

        public PageResults<CourseDto> GetAllEnrolledByUser(GetAllQuery queryParameters)
        {
            var userId = userContextService.GetUserId;
            var spec = new EnrolledCoursesByUserIdWithOwnerSpecification(userId, queryParameters.SearchPhrase);
            var courseQuery = UnitOfWork.CoursesEnrolledUsers.GetBySpecification(spec).Select(f => f.Course);
            var result = queryParametersService.PreparePaginationResults<CourseDto, Course>(queryParameters, courseQuery, mapper);
            foreach (var item in result.Items)
            {
                item.ApprovalStatus = EApprovalStatus.Confirmed;
            }

            return result;
        }

        public PageResults<CourseDto> GetAllPendingByUser(GetAllQuery queryParameters)
        {
            var userId = userContextService.GetUserId;
            var spec = new PendingCoursesByUserIdWithOwnerSpecification(userId, queryParameters.SearchPhrase);
            var courseQuery = UnitOfWork.CoursesPendingUsers.GetBySpecification(spec).Select(f => f.Course);
            var result = queryParametersService.PreparePaginationResults<CourseDto, Course>(queryParameters, courseQuery, mapper);
            foreach (var item in result.Items)
            {
                item.ApprovalStatus = EApprovalStatus.NeedsConfirmation;
            }

            return result;
        }

        public PageResults<CourseDto> GetAllOwnedCoursesByUser(GetAllQuery queryParameters)
        {
            var userId = userContextService.GetUserId;
            var spec = new OwnedCoursesByUserIdWithOwnerSpecification(userId, queryParameters.SearchPhrase);
            var courseQuery = UnitOfWork.Courses.GetBySpecification(spec);
            var result = queryParametersService.PreparePaginationResults<CourseDto, Course>(queryParameters, courseQuery, mapper);
            foreach (var item in result.Items)
            {
                item.ApprovalStatus = EApprovalStatus.None;
            }

            return result;
        }

        public PageResults<CourseDto> GetAllByUser(GetAllQuery queryParameters)
        {
            var resultPending = GetAllPendingByUser(queryParameters);
            var resultEnrolled = GetAllEnrolledByUser(queryParameters);
            var resultOwned = GetAllOwnedCoursesByUser(queryParameters);

            var concatResult = new PageResults<CourseDto>(resultPending.Items.Concat(resultEnrolled.Items).Concat(resultOwned.Items).ToList(),
                resultPending.TotalItemsCount + resultEnrolled.TotalItemsCount,
                queryParameters.PageSize,
                queryParameters.PageNumber);

            return concatResult;
        }
        public CourseMembersDto GetAllMembersWithStatus(int courseId)
        {
            var userId = userContextService.GetUserId;
            var spec = new CourseByIdWithUsersSpecification(courseId);
            var course = UnitOfWork.Courses.GetBySpecification(spec).FirstOrDefault();
            if (course.UserId != userId)
            {
                throw new ForbidException("User has no owner access to this course.");
            }
            var courseMembersDto = mapper.Map<CourseMembersDto>(course);
            return courseMembersDto;

        }
        public CourseDto GetById(int courseId)
        {
            var userId = userContextService.GetUserId;
            Course course = GetCourseIfUserBelongsTo(userId, courseId);
            var courseDto = mapper.Map<CourseDto>(course);
            courseDto.ApprovalStatus = EApprovalStatus.Confirmed;
            return courseDto;
        }

        public void JoinCourse(int courseId)
        {
            var userId = userContextService.GetUserId;
            var spec = new CourseByIdWithUsersSpecification(courseId);
            var course = UnitOfWork.Courses.GetById(courseId);
            var pendingSpec = new PendingCoursesByUserIdWithOwnerSpecification(userId, null);
            var pendingCourses = UnitOfWork.CoursesPendingUsers.GetBySpecification(pendingSpec);
            if (pendingCourses.ToList().Exists(f => f.CourseId == courseId))
            {
                throw new ForbidException("Already pending to course");
            }
            var enrolledSpec = new EnrolledCoursesByUserIdWithOwnerSpecification(userId, null);
            var enrolledCourses = UnitOfWork.CoursesEnrolledUsers.GetBySpecification(enrolledSpec);
            if (enrolledCourses.ToList().Exists(f => f.CourseId == courseId))
            {
                throw new ForbidException("Already joined to course");
            }
            var user = UnitOfWork.Users.GetById(userId);
            var transaction = new JoinCourseTransaction(course, user);
            transaction.Execute();
            UnitOfWork.Commit();
        }

        public void AcceptMember(CourseMemberDto courseMemberDto)
        {
            var userId = userContextService.GetUserId;
            var course = GetCourseIfUserBelongsTo(userId, courseMemberDto.CourseId);
            if (course.UserId != userId)
            {
                throw new ForbidException("User has no owner access to this course.");
            }
            var user = UnitOfWork.Users.GetById(courseMemberDto.UserId);
            var pendingSpec = new PendingCoursesByUserIdWithOwnerSpecification(courseMemberDto.UserId, null);
            var pendingCourses = UnitOfWork.CoursesPendingUsers.GetBySpecification(pendingSpec);
            if (!pendingCourses.ToList().Exists(f => f.CourseId == courseMemberDto.CourseId))
            {
                throw new NotFoundException("User is not pending to this course.");
            }
            var transaction = new AcceptMemberTransaction(course, user);
            transaction.Execute();
            UnitOfWork.Commit();

        }

        public void RemoveMember(CourseMemberDto courseMemberDto)
        {
            var userId = userContextService.GetUserId;
            var course = GetCourseIfUserBelongsTo(userId, courseMemberDto.CourseId);
            if (course.UserId != userId)
            {
                throw new ForbidException("User has no owner access to this course.");
            }
            var user = UnitOfWork.Users.GetById(courseMemberDto.UserId);
            var pendingSpec = new PendingCoursesByUserIdWithOwnerSpecification(courseMemberDto.UserId, null);
            var pendingCourses = UnitOfWork.CoursesPendingUsers.GetBySpecification(pendingSpec);
            var enrolledSpec = new EnrolledCoursesByUserIdWithOwnerSpecification(courseMemberDto.UserId, null);
            var enrolledCourses = UnitOfWork.CoursesEnrolledUsers.GetBySpecification(enrolledSpec);
            if (!pendingCourses.ToList().Exists(f => f.CourseId == courseMemberDto.CourseId) && !enrolledCourses.ToList().Exists(f => f.CourseId == courseMemberDto.CourseId))
            {
                throw new NotFoundException("User is not registered to this course.");
            }
            var transaction = new RemoveMemberTransaction(course, user);
            transaction.Execute();
            UnitOfWork.Commit();
        }

        public Course CreateCourse(AddCourseDto addCourseDto)
        {
            var userId = userContextService.GetUserId;
            Course courseToAdd = mapper.Map<Course>(addCourseDto);
            AddCourseTransaction addNoteTransaction = new(UnitOfWork, userId, courseToAdd);
            addNoteTransaction.Execute();
            UnitOfWork.Commit();
            var newCourse = addNoteTransaction.CourseToAdd;
            logger.Log(EUserAction.CreateCourse, userId, DateTime.UtcNow, newCourse.Id);
            return newCourse;
        }

        public void DeleteCourse(int courseId)
        {
            var userId = userContextService.GetUserId;
            Course courseToRemove = GetCourseIfUserBelongsTo(userId, courseId);

            var authorizationResult = authorizationService.AuthorizeAsync(userContextService.User, courseToRemove,
                new ResourceOperationRequirement(ResourseOperation.Delete)).Result;

            if (authorizationResult.Succeeded)
            {
                DeleteEntityTransaction<Course> deleteCourseTransaction = new(UnitOfWork.Courses, courseToRemove.Id);
                deleteCourseTransaction.Execute();
                UnitOfWork.Commit();
                logger.Log(EUserAction.DeleteCourse, userId, DateTime.UtcNow, courseId);
            }
            else
            {
                throw new ForbidException("Cannot delete this course.");
            }
        }

        public void UpdateCourse(int courseId, UpdateCourseDto updateCourseDto)
        {
            var userId = userContextService.GetUserId;
            Course courseToUpdate = GetCourseIfUserBelongsTo(userId, courseId);

            var authorizationResult = authorizationService.AuthorizeAsync(userContextService.User, courseToUpdate,
                new ResourceOperationRequirement(ResourseOperation.Update)).Result;

            if (authorizationResult.Succeeded)
            {
                UpdateCourseTransaction updateCourseTransaction = new(courseToUpdate, updateCourseDto.Name, updateCourseDto.IconPath, updateCourseDto.YearStart);
                updateCourseTransaction.Execute();
                UnitOfWork.Commit();
                logger.Log(EUserAction.UpdateCourse, userId, DateTime.UtcNow, courseId);
            }
            else
            {
                throw new ForbidException("Cannot update this course.");
            }
        }

        private Course GetCourseIfUserBelongsTo(int userId, int courseId)
        {
            var spec = new CourseByIdWithOwnerSpecification(courseId);
            var course = UnitOfWork.Courses.GetBySpecification(spec).FirstOrDefault();
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
