using AutoMapper;
using API_project_system.Entities;
using System.Security.Cryptography;
using API_project_system.ModelsDto;
using API_project_system.ModelsDto.UserDtos;

namespace API_project_system.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile() 
        {
            CreateMap<RegisterUserDto, User>();
            CreateMap<UpdateUserDto, User>();
            CreateMap<User, UserDto>();
            CreateMap<User, UserNamesDto>();
        }
    }
}
