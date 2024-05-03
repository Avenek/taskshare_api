using API_project_system.Entities;
using API_project_system.Logger;
using API_project_system.ModelsDto.CourseDto;
using API_project_system.ModelsDto;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using API_project_system.Exceptions;
using API_project_system.ModelsDto.SubmissionDto;
using API_project_system.Transactions.Submissions;
using API_project_system.Specifications.AssigmentSpecifications;
using Microsoft.AspNetCore.Http;
using API_project_system.Authorization;
using API_project_system.Transactions;
using API_project_system.Specifications.SubmissionSpecifications;

namespace API_project_system.Services
{
    public interface ISubmissionService
    {
        public IUnitOfWork UnitOfWork { get; }

        public int CreateSubmission(AddSubmissionDto addSubmissionDto);
        public Task UploadFilesToSubmissionAsync(int submissionId, IFormFileCollection filesToUpload);
        public void DeleteSubmission(int submissionId);
        public void UpdateSubmission(int submissionId, UpdateSubmissionDto updateCoursedto);
        void DeleteFileFromSubmission(int fileId);
        SubmissionDto GetById(int submissionId);
    }
    public class SubmissionService : ISubmissionService
    {
        const string STUDENT_FILES_PATH = "student_files";
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

        public int CreateSubmission(AddSubmissionDto addSubmissionDto)
        {
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

        public async Task UploadFilesToSubmissionAsync(int submissionId, IFormFileCollection filesToUpload)
        {
            var userId = userContextService.GetUserId;
            Submission submission = UnitOfWork.Submissions.GetById(submissionId);

            var spec = new AssignmentByIdWithCourseAndOwnerAndEnrolledUsersSpecification(submission.AssignmentId);
            Assignment assignment = UnitOfWork.Assignments.GetBySpecification(spec).FirstOrDefault();
            if (assignment == null)
            {
                throw new BadRequestException("Wrong assignment id.");
            }
            var authorizationResult = authorizationService.AuthorizeAsync(userContextService.User, assignment.Course,
                new UserEnrolledToCourseRequirement()).Result;

            if (authorizationResult.Succeeded)
            {
                string pathToUpload = CreatePathToUpload(assignment, userId);

                if (!Directory.Exists(pathToUpload))
                {
                    Directory.CreateDirectory(pathToUpload);
                }

                foreach (var file in filesToUpload)
                {
                    if (file.Length > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var filePath = Path.Combine(pathToUpload, fileName);

                        UploadSubmissionFileTransaction uploadSubmissionFileTransaction = new UploadSubmissionFileTransaction(UnitOfWork, submissionId, filePath);
                        uploadSubmissionFileTransaction.Execute();
                        UnitOfWork.Commit();
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        logger.Log(EUserAction.UploadFile, userId, DateTime.UtcNow, uploadSubmissionFileTransaction.FileToAdd.Id);
                    }
                    else
                    {
                        throw new BadRequestException("Empty file.");
                    }
                }
            }
            else
            {
                throw new BadRequestException("User has no acces to this course.");
            }
        }

        private string CreateAssignmentDirectoryPath(Assignment assignment) =>
            $"{assignment.Name.Replace(' ', '_')}";

        private string CreateCourseDirectoryPath(Assignment assignment) => 
            $"{assignment.Course.Owner.Lastname}_{assignment.Course.Name.Replace(' ', '_')}_{assignment.Course.YearStart}_{assignment.Course.YearStart + 1}";
        private string CreateStudentDirectoryPath(int userId)
        {
            User student = UnitOfWork.Users.GetById(userId);
            return $"{student.Lastname}_{student.Name}";
        }

        private string CreatePathToUpload(Assignment assignment, int userId)
        {
            string courseDirectoryPath = CreateCourseDirectoryPath(assignment);
            string studentDirectoryPath = CreateStudentDirectoryPath(userId);
            string assignmetDirectoryPath = CreateAssignmentDirectoryPath(assignment);
            return Path.Combine(STUDENT_FILES_PATH, courseDirectoryPath, studentDirectoryPath, assignmetDirectoryPath);
        }

        public void DeleteFileFromSubmission(int fileId)
        {
            var userId = userContextService.GetUserId;
            SubmissionFile fileToRemove = GetFileIfBelongsToUser(userId, fileId);

            DeleteEntityTransaction<SubmissionFile> deleteCourseTransaction = new(UnitOfWork.SubmissionFiles, fileToRemove.Id);
            deleteCourseTransaction.Execute();
            UnitOfWork.Commit();
            File.Delete(fileToRemove.FilePath);
            logger.Log(EUserAction.DeleteFile, userId, DateTime.UtcNow, fileId);
        }

        public void DeleteSubmission(int submissionId)
        {
            var userId = userContextService.GetUserId;
            Submission submissionToRemove = GetSubmissionIfBelongsToUser(userId, submissionId);
            foreach(var file in submissionToRemove.Files)
            {
                File.Delete(file.FilePath);

            }
            DeleteEntityTransaction<Submission> deleteCourseTransaction = new(UnitOfWork.Submissions, submissionToRemove.Id);
            deleteCourseTransaction.Execute();
            UnitOfWork.Commit();
            logger.Log(EUserAction.DeleteSubmission, userId, DateTime.UtcNow, submissionId);
        }

        public void UpdateSubmission(int submissionId, UpdateSubmissionDto updateCoursedto)
        {
            throw new NotImplementedException();
        }

        private SubmissionFile GetFileIfBelongsToUser(int userId, int fileId)
        {
            var spec = new SubmissionFileByIdWithAssignemnt(fileId);
            SubmissionFile submissionFile = UnitOfWork.SubmissionFiles.GetBySpecification(spec).FirstOrDefault();
            if (submissionFile is null)
            {
                throw new NotFoundException("That entity doesn't exist.");
            }
            if (submissionFile.Submission.UserId != userId)
            {
                throw new ForbidException("Cannot access to this file.");
            }

            return submissionFile;
        }

        private Submission GetSubmissionIfBelongsToUser(int userId, int submissionId)
        {
            var spec = new SubmissionByIdWithFiles(submissionId);
            Submission submission = UnitOfWork.Submissions.GetBySpecification(spec).FirstOrDefault();
            if (submission is null)
            {
                throw new NotFoundException("That entity doesn't exist.");
            }
            if (submission.UserId != userId)
            {
                throw new ForbidException("Cannot access to this file.");
            }

            return submission;
        }
    }
}
