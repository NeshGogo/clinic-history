using AutoMapper;
using DoctorService.Dtos;
using DoctorService.Entities;
using AccountService;

namespace DoctorService.Profiles
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            // --> Speciality
            CreateMap<SpecialityDto, Speciality>().ReverseMap();
            CreateMap<SpecialityCreateDto, Speciality>().ReverseMap();

            // --> Doctor
            CreateMap<DoctorDto, Doctor>().ReverseMap();
            CreateMap<DoctorCreateDto, Doctor>().ReverseMap();

            // --> User
            CreateMap<UserPublishMessageDto, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(or => or.Id));
            CreateMap<GrpcUserModel, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(or => or.Id));
        }
    }
}
