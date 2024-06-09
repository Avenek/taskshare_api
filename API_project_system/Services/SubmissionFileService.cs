using API_project_system.Authorization;
using API_project_system.Entities;
using API_project_system.Exceptions;
using API_project_system.Logger;
using API_project_system.ModelsDto.SubmissionDto;
using API_project_system.ModelsDto.SubmissionFileDtos;
using API_project_system.Specifications.AssigmentSpecifications;
using API_project_system.Specifications.CourseSpecifications;
using API_project_system.Specifications.SubmissionSpecifications;
using API_project_system.Transactions;
using API_project_system.Transactions.Submissions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.StaticFiles;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace API_project_system.Services
{
    public interface ISubmissionFileService
    {
        public IUnitOfWork UnitOfWork { get; }
        public Task UploadFilesToSubmissionAsync(int submissionId, IFormFileCollection filesToUpload);
        void DeleteFileFromSubmission(int fileId);
        MemoryStream GetFilesFromAssignment(int assignmentId);
        MemoryStream GetFilesFromUser(int courseId, int userId);
        MemoryStream GetFilesFromCourse(int courseId);
        public Task<FileDto> GetFileFromSubmission(int submissionId, int fileId);

    }
    public class SubmissionFileService : ISubmissionFileService
    {
        const string STUDENT_FILES_PATH = "student_files";
        public IUnitOfWork UnitOfWork { get; }
        private readonly IMapper mapper;
        private readonly UserActionLogger logger;
        private readonly IUserContextService userContextService;
        private readonly IAuthorizationService authorizationService;

        public SubmissionFileService(IUnitOfWork unitOfWork, IMapper mapper, UserActionLogger logger,
            IUserContextService userContextService, IAuthorizationService authorizationService)
        {
            UnitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
            this.userContextService = userContextService;
            this.authorizationService = authorizationService;
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
                            file.CopyTo(stream);
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
            $"{assignment.Name.Replace(' ', '_').Replace(".", string.Empty)}";

        private string CreateCourseDirectoryPath(Course course) =>
            $"{course.Owner.Lastname}_{course.Name.Replace(' ', '_').Replace(".", string.Empty)}_{course.YearStart}_{course.YearStart + 1}";
        private string CreateStudentDirectoryPath(int userId)
        {
            User student = UnitOfWork.Users.GetById(userId);
            return $"{student.Lastname}_{student.Name}";
        }

        private string CreatePathToUpload(Assignment assignment, int userId)
        {
            string courseDirectoryPath = CreateCourseDirectoryPath(assignment.Course);
            string studentDirectoryPath = CreateStudentDirectoryPath(userId);
            string assignmentDirectoryPath = CreateAssignmentDirectoryPath(assignment);
            return Path.Combine(STUDENT_FILES_PATH, courseDirectoryPath, studentDirectoryPath, assignmentDirectoryPath);
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

        public async Task<FileDto> GetFileFromSubmission(int submissionId, int fileId)
        {
            var userId = userContextService.GetUserId;
            var spec = new SubmissionFileByIdWithAssignemnt(fileId);
            var submissionFile = UnitOfWork.SubmissionFiles.GetBySpecification(spec).FirstOrDefault();

            if (submissionFile == null)
            {
                throw new BadRequestException("That file doesn't exist");
            }
            if (submissionFile.Submission.Assignment.UserId != userId && submissionFile.Submission.UserId != userId)
            {
                throw new BadRequestException("Cannot access this file");
            }

            var file = await File.ReadAllBytesAsync(submissionFile.FilePath);

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(submissionFile.FilePath, out string contentType))
            {
                contentType = "application/octet-stream";
            }
            FileDto fileDto = new FileDto { Content = file, ContentType = contentType};

            return fileDto;
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

        public MemoryStream GetFilesFromAssignment(int assignmentId)
        {
            var spec = new AssignmentByIdWithCourseAndOwnerAndEnrolledUsersSpecification(assignmentId);
            Assignment assignment = UnitOfWork.Assignments.GetBySpecification(spec).FirstOrDefault();
            if (assignment == null)
            {
                throw new BadRequestException("Wrong assignment id.");
            }
            var course = UnitOfWork.Courses.GetById(assignment.CourseId);
            var authorizationResult = authorizationService.AuthorizeAsync(userContextService.User, course,
                new ResourceOperationRequirement(ResourseOperation.Update)).Result;

            if (authorizationResult.Succeeded)
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        foreach (var user in assignment.Course.EnrolledUsers)
                        {
                            string userDirectoryPath = CreateStudentDirectoryPath(user.Id);
                            var userDirectoryEntry = archive.CreateEntry(userDirectoryPath + "/");

                            string path = CreatePathToUpload(assignment, user.Id);
                            AddFolderToZip(archive, path, userDirectoryPath);
                        }   

                    }
                    memoryStream.Position = 0;
                    return memoryStream;
                }
            }
            else
            {
                throw new ForbidException("Cannot download files from this course.");
            }
        }

        public MemoryStream GetFilesFromUser(int courseId, int userId)
        {
            var course = UnitOfWork.Courses.GetById(courseId);
            var authorizationResult = authorizationService.AuthorizeAsync(userContextService.User, course,
                new ResourceOperationRequirement(ResourseOperation.Update)).Result;

            if (authorizationResult.Succeeded)
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        string courseDirectoryPath = CreateCourseDirectoryPath(course);
                        string studentDirectoryPath = CreateStudentDirectoryPath(userId);
                        string path = Path.Combine(courseDirectoryPath, courseDirectoryPath);
                        AddFolderToZip(archive, path, string.Empty);
                    }
                    memoryStream.Position = 0;
                    return memoryStream;
                }
            }
            else
            {
                throw new ForbidException("Cannot download files from this course.");
            }
        }

        public MemoryStream GetFilesFromCourse(int courseId)
        {
            var spec = new CourseByIdWithOwnerSpecification(courseId);
            var course = UnitOfWork.Courses.GetBySpecification(spec).FirstOrDefault();
            if (course is null)
            {
                throw new NotFoundException("That entity doesn't exist.");
            }
            var authorizationResult = authorizationService.AuthorizeAsync(userContextService.User, course,
                new ResourceOperationRequirement(ResourseOperation.Update)).Result;

            if (authorizationResult.Succeeded)
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        string courseDirectoryPath = Path.Combine(STUDENT_FILES_PATH, CreateCourseDirectoryPath(course));
                        AddFolderToZip(archive, courseDirectoryPath, string.Empty);
                    }
                    memoryStream.Position = 0;
                    return memoryStream;
                }
            }
            else
            {
                throw new ForbidException("Cannot download files from this course.");
            }
        }

        private void AddFolderToZip(ZipArchive archive, string sourceFolder, string entryName)
        {
            var directoryInfo = new DirectoryInfo(sourceFolder);

            foreach (var file in directoryInfo.GetFiles())
            {
                var entry = archive.CreateEntry(Path.Combine(entryName, file.Name));
                using (var entryStream = entry.Open())
                using (var fileStream = file.OpenRead())
                {
                    fileStream.CopyTo(entryStream);
                }
            }

            foreach (var directory in directoryInfo.GetDirectories())
            {
                AddFolderToZip(archive, directory.FullName, Path.Combine(entryName, directory.Name));
            }
        }
    }
}
