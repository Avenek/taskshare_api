﻿using API_project_system.ModelsDto;
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

        public CourseController(ICourseService courseService)
        {
            this.courseService = courseService;
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

        [HttpGet("user/owned")]
        [Authorize(Roles = "Teacher")]
        [Authorize(Policy = "IsConfirmed")]
        public ActionResult GetAllOwnedByUser([FromQuery] GetAllQuery queryParameters)
        {
            var courses = courseService.GetAllOwnedByUser(queryParameters);
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

        [HttpPut("join/{courseId}")]
        [Authorize(Roles = "Student")]
        public ActionResult JoinCourse(int courseId)
        {
            courseService.JoinCourse(courseId);
            return Ok();

        }

        [HttpPut("accept")]
        [Authorize(Roles = "Teacher")]
        [Authorize(Policy = "IsConfirmed")]
        public ActionResult AcceptMember([FromBody] CourseMemberDto acceptMemberDto)
        {
            courseService.AcceptMember(acceptMemberDto);
            return Ok();

        }

        [HttpPut("remove")]
        [Authorize(Roles = "Teacher")]
        [Authorize(Policy = "IsConfirmed")]
        public ActionResult RemoveMember([FromBody] CourseMemberDto removeMemberDto)
        {
            courseService.RemoveMember(removeMemberDto);
            return Ok();

        }

        [HttpGet("members/{courseId}")]
        [Authorize(Roles = "Teacher")]
        [Authorize(Policy = "IsConfirmed")]
        public ActionResult GetAllMembersWithStatus(int courseId)
        {
            var courseMembersDto = courseService.GetAllMembersWithStatus(courseId);
            return Ok(courseMembersDto);
        }

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [Authorize(Policy = "IsConfirmed")]
        public ActionResult CreateCourse([FromBody] AddCourseDto courseToAdd)
        {
            var course = courseService.CreateCourse(courseToAdd);
            return Created($"api/course/{course.Id}", course);

        }

        [HttpDelete("{courseId}")]
        [Authorize(Roles = "Teacher")]
        [Authorize(Policy = "IsConfirmed")]
        public ActionResult DeleteCourse(int courseId)
        {
            courseService.DeleteCourse(courseId);
            return NoContent();

        }

        [HttpPut("{courseId}")]
        [Authorize(Roles = "Teacher")]
        [Authorize(Policy = "IsConfirmed")]
        public ActionResult UpdateCourse(int courseId, [FromBody] UpdateCourseDto updateCoursedto)
        {
            courseService.UpdateCourse(courseId, updateCoursedto);
            return Ok();

        }
    }
}
