using API_project_system.Entities;
using API_project_system.ModelsDto.AssignmentDto;
using AutoMapper;

namespace API_project_system.MappingProfiles
{
    public class AssignmentMappingProfile : Profile
    {
        public AssignmentMappingProfile()
        {
            CreateMap<Assignment, AssignmentDto>();
            CreateMap<AssignmentDto, Assignment>();
            CreateMap<AddAssignmentDto, Assignment>();
            CreateMap<UpdateAssignmentDto, Assignment>();
        }
    }
}