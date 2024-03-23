using AutoMapper;
using API_project_system.Entities;
using API_project_system.ModelsDto;
using System.Security.Cryptography;

namespace API_project_system.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile() 
        {
            CreateMap<RegisterUserDto, User>();
            CreateMap<UpdateUserDto, User>();
        }
    }
}
