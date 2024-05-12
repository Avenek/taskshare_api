using API_project_system.ModelsDto.AssignmentDto;
using API_project_system.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_project_system.Controllers
{
    [Route("api/assignment")]
    [ApiController]
    [Authorize]
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentService assignmentService;
        public AssignmentController(IAssignmentService assignmentService)
        {
            this.assignmentService = assignmentService;
        }
        [HttpGet("bycourse/{courseId}")]
        public ActionResult GetAllByCourseId(int courseId)
        {
            var assignments = assignmentService.GetAllByCourseId(courseId);
            return Ok(assignments);
        }
        [HttpGet("{assignmentId}")]
        public ActionResult GetById(int assignmentId)
        {
            var assignment = assignmentService.GetById(assignmentId);
            return Ok(assignment);
        }
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public ActionResult CreateAssignment([FromBody] AddAssignmentDto assignmentToAdd)
        {
            var assignment = assignmentService.CreateAssignment(assignmentToAdd);
            return Created($"api/assignment/{assignment.Id}", assignment);
        }

        [HttpDelete("{assignmentId}")]
        [Authorize(Roles = "Teacher")]
        public ActionResult DeleteAssignment(int assignmentId)
        {
            assignmentService.DeleteAssignment(assignmentId);
            return NoContent();
        }

        [HttpPut("{assignmentId}")]
        [Authorize(Roles = "Teacher")]
        public ActionResult UpdateAssignment(int assignmentId, [FromBody] UpdateAssignmentDto updateAssignmentDto)
        {
            assignmentService.UpdateAssignment(assignmentId, updateAssignmentDto);
            return Ok();
        }
    }
}