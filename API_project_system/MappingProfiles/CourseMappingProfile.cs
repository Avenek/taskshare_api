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
            CreateMap<Course, CourseDto>()
                        .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.Owner));
            CreateMap<Course, CourseMembersDto>()
                        .ForMember(dest => dest.EnrolledUsers, opt => opt.MapFrom(src => src.EnrolledUsers))
                        .ForMember(dest => dest.PendingUsers, opt => opt.MapFrom(src => src.PendingUsers));

            CreateMap<User, UserDto>();
        }
    }
}
