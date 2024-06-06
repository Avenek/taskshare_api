using API_project_system.Entities;
using API_project_system.ModelsDto.AssigmentDto;
using API_project_system.ModelsDto.SubmissionDto;
using AutoMapper;

namespace API_project_system.MappingProfiles
{
    public class SubmissionMappingProfile : Profile
    {
        public SubmissionMappingProfile()
        {
            CreateMap<AddSubmissionDto, Submission>();
            CreateMap<UpdateSubmissionDto, Submission>();
            CreateMap<Submission, SubmissionDto>()
                .ForMember(dest => dest.Files, opt => opt.MapFrom(src => src.Files))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));
            CreateMap<Submission, AssignmentWithSubmissionsDto>();
        }
    }
}
