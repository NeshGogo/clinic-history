using AutoMapper;
using HistoryService.Dtos;
using HistoryService.Entities;

namespace HistoryService.Profiles
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // --> User
            CreateMap<UserPublishMessageDto, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(or => or.Id));

            // --> Doctor
            CreateMap<DoctorPublishMessageDto, Doctor>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(or => or.Id));
        }
    }
}
