using AutoMapper;
using DoctorService.Dtos;
using DoctorService.Entities;

namespace DoctorService.Profiles
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<SpecialityDto, Speciality>().ReverseMap();
        }
    }
}
