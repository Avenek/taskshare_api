using API_project_system.Entities;
using API_project_system.ModelsDto.SubmissionFileDtos;
using AutoMapper;

namespace API_project_system.MappingProfiles
{
    public class SubmissionFileMappingProfile : Profile
    {
        public SubmissionFileMappingProfile()
        {
            CreateMap<SubmissionFile, SubmissionFileDto>()
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(dest => Path.GetFileName(dest.FilePath)));
        }
        
    }
}
