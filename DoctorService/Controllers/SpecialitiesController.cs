using AutoMapper;
using DoctorService.Data;
using DoctorService.Dtos;
using DoctorService.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DoctorService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialitiesController : ControllerBase
    {
        private readonly IBaseRepository<Speciality> _repository;
        private readonly IMapper _mapper;

        public SpecialitiesController(IBaseRepository<Speciality> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<SpecialityDto>> Get()
        {
            var results = _repository.Get();
            return _mapper.Map<List<SpecialityDto>>(results);
        }
    }
}
