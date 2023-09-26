using AutoMapper;
using DoctorService.Data;
using DoctorService.Dtos;
using DoctorService.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DoctorService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IBaseRepository<Doctor> _repository;
        private readonly IMapper _mapper;

        public DoctorsController(IBaseRepository<Doctor> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<DoctorDto>> GetAll()
        {
            var results = _repository.Get();
            return _mapper.Map<List<DoctorDto>>(results);
        }

        [HttpGet("{id}", Name = "GetDoctorById")]
        public ActionResult<DoctorDto> GetById(string id)
        {
            var result = _repository.Get(id);
            return _mapper.Map<DoctorDto>(result);
        }

        [HttpPost]
        public ActionResult<DoctorDto> Post([FromBody] DoctorCreateDto createDto)
        {
            var exists = _repository.Exists(p => p.Identification.ToLower() == createDto.Identification.ToLower());
            if (exists) return BadRequest($"Already exists a doctor with the identification: {createDto.Identification}");
            var entity = _mapper.Map<Doctor>(createDto);
            _repository.Add(entity);
            _repository.SaveChanges();

            return CreatedAtRoute(nameof(GetById), new { id = entity.Id }, _mapper.Map<DoctorDto>(entity));
        }

        [HttpPut("{id}")]
        public ActionResult Put(string id, [FromBody] DoctorCreateDto createDto)
        {
            var entity = _repository.Get(id);
            if (entity == null) NotFound($"Could not get a doctor with id: {id}");

            var exists = _repository.Exists(p => p.Identification.ToLower() == createDto.Identification.ToLower() && p.Id != id);
            if (exists) return BadRequest($"Already exists a doctor with the identification:  {createDto.Identification}");

            _mapper.Map(createDto, entity);
            _repository.Update(entity);
            _repository.SaveChanges();

            return NoContent();
        }

        [HttpPut("ActiveOrDisactive/{id}")]
        public ActionResult ActiveOrDisactive(string id)
        {
            var exists = _repository.Exists(p => p.Id == id);
            if (!exists) NotFound($"Could not get a doctor with id: {id}");
            _repository.ActiveOrDisactive(id);
            _repository.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var exists = _repository.Exists(p => p.Id == id);
            if (!exists) NotFound($"Could not get a doctor with id: {id}");
            _repository.Delete(id);
            _repository.SaveChanges();
            return NoContent();
        }
    }
}
