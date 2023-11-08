using AccountService.DTOs;
using AccountService.Entities;
using AutoMapper;

namespace AccountService.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserCreateDTO, User>();
            CreateMap<User, UserDTO>();
            CreateMap<UserCreateDTO, UserInfoDTO>();
            CreateMap<User, UserPublishDTO>();
            CreateMap<User, GrpcUserModel>();
        }
    }
}
