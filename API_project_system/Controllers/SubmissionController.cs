using API_project_system.ModelsDto.SubmissionDto;
using API_project_system.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_project_system.Controllers
{
    [Route("api/submission")]
    [ApiController]
    [Authorize]
    public class SubmissionController : ControllerBase
    {
        private readonly ISubmissionService submissionService;

        public SubmissionController(ISubmissionService submissionService)
        {
            this.submissionService = submissionService;
        }

        [HttpGet("{submissionId}")]
        public ActionResult GetById(int submissionId)
        {
            var submission = submissionService.GetById(submissionId);
            return Ok(submission);
        }

        [HttpGet("byassignment/{assignmentId}")]
        [Authorize(Roles = "Teacher")]
        public ActionResult GetAllByAssignmentId(int assignmentId)
        {
            var submissions = submissionService.GetAllByAssignmentId(assignmentId);
            return Ok(submissions);
        }

        [HttpPost()]
        public ActionResult CreateSubmission([FromBody] AddSubmissionDto addSubmissionDto)
        {
            var submissionId = submissionService.CreateSubmission(addSubmissionDto);
            return Created($"api/submission/{submissionId}", submissionId);
        }

        [HttpPost("{submissionId}/files")]
        public ActionResult UploadFilesToSubmission(int submissionId)
        {
            var files = Request.Form.Files;
            submissionService.UploadFilesToSubmissionAsync(submissionId, files);
            return Ok();
        }

        [HttpDelete("{submissionId}/file/{fileId}")]
        public ActionResult DeleteFileFromSubmission(int submissionId, int fileId)
        {
            submissionService.DeleteFileFromSubmission(fileId);
            return NoContent();
        }


        [HttpGet("{submissionId}/file/{fileId}")]
        public async Task<ActionResult> GetFileFromSubmission(int submissionId, int fileId)
        {
            var file = await submissionService.GetFileFromSubmission(submissionId, fileId);
            return File(file, "file");
        }

        [HttpDelete("{submissionId}")]
        public ActionResult DeleteSubmission(int submissionId)
        {
            submissionService.DeleteSubmission(submissionId);
            return NoContent();
        }

        [HttpPut("{submissionId}")]
        public ActionResult UpdateSubmission(int submissionId, [FromBody] UpdateSubmissionDto updateSubmissionDto)
        {
            submissionService.UpdateSubmission(submissionId, updateSubmissionDto);
            return Ok();

        }
    }
}
