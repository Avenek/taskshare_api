using API_project_system.Entities;
using API_project_system.ModelsDto;
using API_project_system.ModelsDto.CourseDto;
using AutoMapper;

namespace API_project_system.MappingProfiles
{
    public class CourseMappingProfile : Profile
    {
        public CourseMappingProfile()
        {
            CreateMap<AddCourseDto, Course>();
            CreateMap<UpdateCourseDto, Course>();
            CreateMap<Course, CourseDto>();
        }
    }
}
