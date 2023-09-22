using AutoMapper;
using DoctorService.Dtos;
using DoctorService.Entities;

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
        }
    }
}
