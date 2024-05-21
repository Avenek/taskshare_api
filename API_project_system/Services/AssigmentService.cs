using API_project_system.Authorization;
using API_project_system.Entities;
using API_project_system.Exceptions;
using API_project_system.Logger;
using API_project_system.ModelsDto.AssignmentDto;
using API_project_system.Specifications.AssigmentSpecifications;
using API_project_system.Specifications.CourseSpecifications;
using API_project_system.Transactions;
using API_project_system.Transactions.Assignments;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace API_project_system.Services
{
    public interface IAssignmentService
    {
        public IUnitOfWork UnitOfWork { get; }
        public List<AssignmentDto> GetAllByCourseId(int courseId);
        public AssignmentDto GetById(int assignmentId);
        public AssignmentDto CreateAssignment(AddAssignmentDto assignment);
        public void DeleteAssignment(int assignmentId);
        public void UpdateAssignment(int assignmentId, UpdateAssignmentDto updateAssignmentDto);
    }

    public class AssigmentService : IAssignmentService
    {
        public IUnitOfWork UnitOfWork { get; }
        private readonly IMapper mapper;
        private readonly UserActionLogger logger;
        private readonly IUserContextService userContextService;
        private readonly IAuthorizationService authorizationService;

        public AssigmentService(IUnitOfWork unitOfWork, IMapper mapper, UserActionLogger logger,
            IUserContextService userContextService, IAuthorizationService authorizationService)
        {
            UnitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
            this.userContextService = userContextService;
            this.authorizationService = authorizationService;
        }
        public List<AssignmentDto> GetAllByCourseId(int courseId)
        {
            if (!(IsOwner(courseId) || IsEnrolled(courseId)))
            {
                throw new ForbidException("User has not access to this course");
            }
            var userId = userContextService.GetUserId;
            var spec = new AssignmentsByCourseIdSpecification(courseId, userId);

            var assignments = UnitOfWork.Assignments.GetBySpecification(spec);
            var assignmentDtos = mapper.Map<List<AssignmentDto>>(assignments);
            return assignmentDtos;

        }
        public AssignmentDto GetById(int assigmentId)
        {
            var assignment = GetAssignmentIfUserBelongsTo(assigmentId);
            var assignmentDto = mapper.Map<AssignmentDto>(assignment);
            return assignmentDto;
        }

        public AssignmentDto CreateAssignment(AddAssignmentDto assignmentToAdd)
        {
            var userId = userContextService.GetUserId;
            if (!IsOwner(assignmentToAdd.CourseId))
            {
                throw new ForbidException("Cannot create assignment for this course.");
            }

            var assignment = mapper.Map<Assignment>(assignmentToAdd);

            var transaction = new AddAssignmentTransaction(UnitOfWork, assignment);
            transaction.Execute();
            UnitOfWork.Commit();

            var assignmentDto = mapper.Map<AssignmentDto>(assignment);
            logger.Log(EUserAction.CreateAssignment, userId, DateTime.UtcNow, assignment.Id);

            return assignmentDto;
        }
        public void DeleteAssignment(int assignmentId)
        {
            var userId = userContextService.GetUserId;
            var assignment = GetAssignmentIfUserBelongsTo(assignmentId);

            var authorizationResult = authorizationService.AuthorizeAsync(userContextService.User, assignment,
                                new ResourceOperationRequirement(ResourseOperation.Delete)).Result;

            if (authorizationResult.Succeeded)
            {
                DeleteEntityTransaction<Assignment> deleteAssignmentTransaction = new(UnitOfWork.Assignments, assignment.Id);
                deleteAssignmentTransaction.Execute();
                UnitOfWork.Commit();
                logger.Log(EUserAction.DeleteAssignment, userId, DateTime.UtcNow, assignmentId);
            }
            else
            {
                throw new ForbidException("Cannot delete this assignment.");
            }
        }
        public void UpdateAssignment(int assignmentId, UpdateAssignmentDto updateAssignmentDto)
        {
            var userId = userContextService.GetUserId;
            var assignment = GetAssignmentIfUserBelongsTo(assignmentId);

            var authorizationResult = authorizationService.AuthorizeAsync(userContextService.User, assignment,
                                                new ResourceOperationRequirement(ResourseOperation.Update)).Result;

            if (authorizationResult.Succeeded)
            {
                var transaction = new UpdateAssignmentTransaction(assignment, updateAssignmentDto);
                transaction.Execute();
                UnitOfWork.Commit();
                logger.Log(EUserAction.UpdateAssignment, userId, DateTime.UtcNow, assignmentId);
            }
            else
            {
                throw new ForbidException("Cannot update this assignment.");
            }
        }

        private Assignment GetAssignmentIfUserBelongsTo(int assignmentId)
        {
            var assignment = UnitOfWork.Assignments.GetById(assignmentId);

            if (!(IsOwner(assignment.CourseId) || IsEnrolled(assignment.CourseId)))
            {
                throw new ForbidException("Cannot access this assigment.");
            }
            if (!IsOwner(assignment.CourseId) && !assignment.Visibility)
            {
                throw new ForbidException("Cannot access this assigment.");
            }

            return assignment;
        }

        private bool IsOwner(int courseId)
        {
            var userId = userContextService.GetUserId;
            var course = UnitOfWork.Courses.GetById(courseId);
            return course.UserId == userId;
        }
        private bool IsEnrolled(int courseId)
        {
            var userId = userContextService.GetUserId;
            var enrolledSpec = new EnrolledCoursesByUserIdWithOwnerSpecification(userId, null);
            var enrolledCourses = UnitOfWork.CoursesEnrolledUsers.GetBySpecification(enrolledSpec);
            return enrolledCourses.ToList().Exists(f => f.CourseId == courseId);
        }
    }
}