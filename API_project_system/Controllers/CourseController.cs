using API_project_system.ModelsDto;
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
        private readonly ICourseService repositoryService;

        public CourseController(ICourseService repositoryService)
        {
            this.repositoryService = repositoryService;
        }

        [HttpGet]
        public ActionResult GetAll([FromQuery] GetAllQuery queryParameters)
        {
            var courses = repositoryService.GetAll(queryParameters);
            return Ok(courses);
        }

        [HttpGet("user/enrolled")]
        public ActionResult GetAllEnrolledByUser([FromQuery] GetAllQuery queryParameters)
        {
            var courses = repositoryService.GetAllEnrolledByUser(queryParameters);
            return Ok(courses);
        }

        [HttpGet("user/pending")]
        public ActionResult GetAllPendingByUser([FromQuery] GetAllQuery queryParameters)
        {
            var courses = repositoryService.GetAllPendingByUser(queryParameters);
            return Ok(courses);
        }

        [HttpGet("user")]
        public ActionResult GetAllByUser([FromQuery] GetAllQuery queryParameters)
        {
            var courses = repositoryService.GetAllByUser(queryParameters);
            return Ok(courses);
        }

        [HttpGet("{repositoryId}")]
        public ActionResult GetById(int repositoryId)
        {
            var courses = repositoryService.GetById(repositoryId);
            return Ok(courses);
        }

    }
}
