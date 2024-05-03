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

namespace API_project_system.Services
{
    public interface ISubmissionService
    {
        public IUnitOfWork UnitOfWork { get; }

        public int CreateSubmission(AddSubmissionDto addSubmissionDto);
        public Task UploadFilesToSubmissionAsync(int submissionId, IFormFileCollection filesToUpload);
        public void DeleteSubmission(int submissionId);
        public void UpdateSubmission(int submissionId, UpdateSubmissionDto updateCoursedto);
    }
    public class SubmissionService : ISubmissionService
    {
        const string STUDENT_FILES_PATH = "student_files";
        public IUnitOfWork UnitOfWork { get; }
        private readonly IMapper mapper;
        private readonly UserActionLogger logger;
        private readonly IPaginationService queryParametersService;
        private readonly IUserContextService userContextService;
        private readonly IAuthorizationService authorizationService;

        public SubmissionService(IUnitOfWork unitOfWork, IMapper mapper, UserActionLogger logger,
            IPaginationService queryParametersService, IUserContextService userContextService, IAuthorizationService authorizationService)
        {
            UnitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
            this.queryParametersService = queryParametersService;
            this.userContextService = userContextService;
            this.authorizationService = authorizationService;
        }

        public int CreateSubmission(AddSubmissionDto addSubmissionDto)
        {
            var userId = userContextService.GetUserId;
            Assignment assignment = UnitOfWork.Assignments.GetById(addSubmissionDto.AssignmentId);
            if (assignment == null)
            {
                throw new BadRequestException("Wrong assignment id.");
            }
            Submission submissionToAdd = mapper.Map<Submission>(addSubmissionDto);
            AddSubmissionTransaction addSubmissionTransaction = new(UnitOfWork, userId, submissionToAdd);
            addSubmissionTransaction.Execute();
            UnitOfWork.Commit();
            var newSubmissionId = addSubmissionTransaction.SubmissionToAdd.Id;
            logger.Log(EUserAction.AddSubmission, userId, DateTime.UtcNow, newSubmissionId);

            return addSubmissionTransaction.SubmissionToAdd.Id;
        }

        public async Task UploadFilesToSubmissionAsync(int submissionId, IFormFileCollection filesToUpload)
        {
            Submission submission = UnitOfWork.Submissions.GetById(submissionId);

            var spec = new AssignmentByIdWithCourseAndOwnerSpecification(submission.AssignmentId);
            Assignment assignment = UnitOfWork.Assignments.GetBySpecification(spec).FirstOrDefault();

            string courseDirectoryPath = CreateCourseDirectoryPath(assignment);
            string studentDirectoryPath = CreateStudentDirectoryPath(submission.UserId);
            string assignmetDirectoryPath = CreateAssignmentDirectoryPath(assignment);

            string pathToUpload = Path.Combine(STUDENT_FILES_PATH, courseDirectoryPath, studentDirectoryPath, assignmetDirectoryPath);

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

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                else
                {
                    throw new BadRequestException("Empty file.");
                }
            }
            UnitOfWork.Commit();
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

        public void DeleteSubmission(int submissionId)
        {
            throw new NotImplementedException();
        }

        public void UpdateSubmission(int submissionId, UpdateSubmissionDto updateCoursedto)
        {
            throw new NotImplementedException();
        }
    }
}
