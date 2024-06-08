using API_project_system.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace API_project_system.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize]
    public class SubmissionFileController : ControllerBase
    {
        private readonly ISubmissionFileService submissionFileService;

        public SubmissionFileController(ISubmissionFileService submissionFileService)
        {
            this.submissionFileService = submissionFileService;
        }

        [HttpPost("submission/{submissionId}/files")]
        public ActionResult UploadFilesToSubmission(int submissionId)
        {
            var files = Request.Form.Files;
            submissionFileService.UploadFilesToSubmissionAsync(submissionId, files);
            return Ok();
        }

        [HttpDelete("submission/{submissionId}/file/{fileId}")]
        public ActionResult DeleteFileFromSubmission(int submissionId, int fileId)
        {
            submissionFileService.DeleteFileFromSubmission(fileId);
            return NoContent();
        }

        [HttpGet("submission/{submissionId}/file/{fileId}")]
        public async Task<ActionResult> GetFileFromSubmission(int submissionId, int fileId)
        {
            var file = await submissionFileService.GetFileFromSubmission(submissionId, fileId);

            return File(file.Content, file.ContentType);
        }

        [HttpGet("assignment/{assignmentId}/files")]
        [Authorize(Roles = "Teacher")]
        [Authorize(Policy = "IsConfirmed")]
        public ActionResult GetFilesFromAssginment(int assignmentId)
        {
            var file = submissionFileService.GetFilesFromAssignment(assignmentId);

            return File(file.ToArray(), "application/zip", "demo.zip");
        }

        [HttpGet("course/{courseId}/user/{userId}/files")]
        [Authorize(Roles = "Teacher")]
        [Authorize(Policy = "IsConfirmed")]
        public ActionResult GetFilesFromUser(int courseId, int userId)
        {
            var file = submissionFileService.GetFilesFromUser(courseId, userId);

            return File(file.ToArray(), "application/zip", "demo.zip");
        }


        [HttpGet("course/{courseId}/files")]
        [Authorize(Roles = "Teacher")]
        [Authorize(Policy = "IsConfirmed")]
        public ActionResult GetFilesFromCourse(int courseId)
        {
            var file = submissionFileService.GetFilesFromCourse(courseId);

            return File(file.ToArray(), "application/zip", "demo.zip");
        }
    }
}
