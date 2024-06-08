using API_project_system.Authorization;
using API_project_system.Entities;
using API_project_system.Exceptions;
using API_project_system.Logger;
using API_project_system.ModelsDto.SubmissionDto;
using API_project_system.ModelsDto.SubmissionFileDtos;
using API_project_system.Specifications.AssigmentSpecifications;
using API_project_system.Specifications.SubmissionSpecifications;
using API_project_system.Transactions;
using API_project_system.Transactions.Submissions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.StaticFiles;

namespace API_project_system.Services
{
    public interface ISubmissionService
    {
        public IUnitOfWork UnitOfWork { get; }
        public int CreateSubmission(AddSubmissionDto addSubmissionDto);
        public void DeleteSubmission(int submissionId);
        public void UpdateSubmission(int submissionId, UpdateSubmissionDto updateSubmissionDto);
        public SubmissionDto GetById(int submissionId);
        public List<SubmissionDto> GetAllByAssignmentId(int assignmentId);
    }
    public class SubmissionService : ISubmissionService
    {
        public IUnitOfWork UnitOfWork { get; }
        private readonly IMapper mapper;
        private readonly UserActionLogger logger;
        private readonly IUserContextService userContextService;
        private readonly IAuthorizationService authorizationService;

        public SubmissionService(IUnitOfWork unitOfWork, IMapper mapper, UserActionLogger logger,
            IUserContextService userContextService, IAuthorizationService authorizationService)
        {
            UnitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
            this.userContextService = userContextService;
            this.authorizationService = authorizationService;
        }
        public SubmissionDto GetById(int submissionId)
        {
            var userId = userContextService.GetUserId;
            Submission submission = GetSubmissionIfBelongsToUser(userId, submissionId);
            submission.Files.Select(f => f.FilePath = Path.GetFileName(f.FilePath));
            var submissionDto = mapper.Map<SubmissionDto>(submission);

            return submissionDto;
        }

        public List<SubmissionDto> GetAllByAssignmentId(int assignmentId)
        {
            var userId = userContextService.GetUserId;
            var spec = new AssignmentByIdWithCourseAndSubmissionsSpecification(assignmentId);
            var assignment = UnitOfWork.Assignments.GetBySpecification(spec).FirstOrDefault(); ;
            if (assignment.Course.UserId != userId)
            {
                throw new ForbidException("User has no owner access to this course.");
            }

            List<Submission> submissions = assignment.Submissions.ToList();
            List<SubmissionDto> submissionDtos = mapper.Map<List<SubmissionDto>>(submissions);

            return submissionDtos;
        }

        public int CreateSubmission(AddSubmissionDto addSubmissionDto)
        {
            if (userContextService.User.IsInRole("Teacher"))
            {
                throw new BadRequestException("Teacher cannot create submission.");
            }

            var userId = userContextService.GetUserId;

            var spec = new AssignmentByIdWithCourseAndOwnerAndEnrolledUsersSpecification(addSubmissionDto.AssignmentId);
            Assignment assignment = UnitOfWork.Assignments.GetBySpecification(spec).FirstOrDefault();
            if (assignment == null)
            {
                throw new BadRequestException("Wrong assignment id.");
            }
            var authorizationResult = authorizationService.AuthorizeAsync(userContextService.User, assignment.Course,
                new UserEnrolledToCourseRequirement()).Result;

            if (authorizationResult.Succeeded)
            {
                if (AlreadySubmitted(addSubmissionDto.AssignmentId, userId))
                {
                    throw new BadRequestException("Assignment is already submitted.");
                }
                Submission submissionToAdd = mapper.Map<Submission>(addSubmissionDto);
                AddSubmissionTransaction addSubmissionTransaction = new(UnitOfWork, userId, submissionToAdd);
                addSubmissionTransaction.Execute();
                UnitOfWork.Commit();
                var newSubmissionId = addSubmissionTransaction.SubmissionToAdd.Id;
                logger.Log(EUserAction.AddSubmission, userId, DateTime.UtcNow, newSubmissionId);

                return addSubmissionTransaction.SubmissionToAdd.Id;
            }
            else
            {
                throw new BadRequestException("User has no acces to this course.");
            }
        }

        private bool AlreadySubmitted(int assignmentId, int userId)
        {
            var submissionSpec = new GetSubmissionByUserAndAssignmentId(assignmentId, userId);
            Submission userSubmission = UnitOfWork.Submissions.GetBySpecification(submissionSpec).FirstOrDefault();

            return userSubmission != null;
        }

        public void DeleteSubmission(int submissionId)
        {
            var userId = userContextService.GetUserId;
            Submission submissionToRemove = GetSubmissionIfBelongsToUser(userId, submissionId);
            foreach (var file in submissionToRemove.Files)
            {
                File.Delete(file.FilePath);

            }
            DeleteEntityTransaction<Submission> deleteCourseTransaction = new(UnitOfWork.Submissions, submissionToRemove.Id);
            deleteCourseTransaction.Execute();
            UnitOfWork.Commit();
            logger.Log(EUserAction.DeleteSubmission, userId, DateTime.UtcNow, submissionId);
        }

        public void UpdateSubmission(int submissionId, UpdateSubmissionDto updateSubmissionDto)
        {
            var userId = userContextService.GetUserId;
            Submission submissionToUpdate = GetSubmissionIfBelongsToUser(userId, submissionId);
            UpdateSubmissionTransaction updateSubmissionTransaction = new(submissionToUpdate, updateSubmissionDto.StudentComment);
            updateSubmissionTransaction.Execute();
            UnitOfWork.Commit();
            logger.Log(EUserAction.UpdateSubmission, userId, DateTime.UtcNow, submissionId);
        }

        private Submission GetSubmissionIfBelongsToUser(int userId, int submissionId)
        {
            var spec = new SubmissionByIdWithFiles(submissionId);
            Submission submission = UnitOfWork.Submissions.GetBySpecification(spec).FirstOrDefault();
            if (submission is null)
            {
                throw new NotFoundException("That entity doesn't exist.");
            }
            if (submission.UserId != userId && submission.Assignment.UserId != userId)
            {
                throw new ForbidException("Cannot access to this submission.");
            }

            return submission;
        }
    }
}
