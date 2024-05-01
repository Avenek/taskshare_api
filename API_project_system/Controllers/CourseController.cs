using API_project_system.ModelsDto;
using API_project_system.ModelsDto.CourseDto;
using API_project_system.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_project_system.Controllers
{
    [Route("api/course")]
    [ApiController]
    [Authorize]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService courseService;

        public CourseController(ICourseService repositoryService)
        {
            this.courseService = repositoryService;
        }

        [HttpGet]
        public ActionResult GetAll([FromQuery] GetAllQuery queryParameters)
        {
            var courses = courseService.GetAll(queryParameters);
            return Ok(courses);
        }

        [HttpGet("user/enrolled")]
        public ActionResult GetAllEnrolledByUser([FromQuery] GetAllQuery queryParameters)
        {
            var courses = courseService.GetAllEnrolledByUser(queryParameters);
            return Ok(courses);
        }

        [HttpGet("user/pending")]
        public ActionResult GetAllPendingByUser([FromQuery] GetAllQuery queryParameters)
        {
            var courses = courseService.GetAllPendingByUser(queryParameters);
            return Ok(courses);
        }

        [HttpGet("user")]
        public ActionResult GetAllByUser([FromQuery] GetAllQuery queryParameters)
        {
            var courses = courseService.GetAllByUser(queryParameters);
            return Ok(courses);
        }

        [HttpGet("{courseId}")]
        public ActionResult GetById(int courseId)
        {
            var courses = courseService.GetById(courseId);
            return Ok(courses);
        }

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public ActionResult CreateCourse([FromBody] AddCourseDto courseToAdd)
        {
            var course = courseService.CreateCourse(courseToAdd);
            return Created($"api/course/{course.Id}", course);

        }

        [HttpDelete("{courseId}")]
        [Authorize(Roles = "Teacher")]
        public ActionResult DeleteCourse(int courseId)
        {
            courseService.DeleteCourse(courseId);
            return NoContent();

        }

        [HttpPut("{courseId}")]
        [Authorize(Roles = "Teacher")]
        public ActionResult UpdateCourse(int courseId, [FromBody] UpdateCourseDto updateCoursedto)
        {
            courseService.UpdateCourse(courseId, updateCoursedto);
            return Ok();

        }
    }
}
