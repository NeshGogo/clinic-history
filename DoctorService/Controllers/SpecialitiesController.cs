using AutoMapper;
using DoctorService.Data;
using DoctorService.Dtos;
using DoctorService.Entities;
using DoctorService.Helppers;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("{id}", Name = "GetSpecialityById")]
        public ActionResult<SpecialityDto> GetById(string id)
        {
            var result = _repository.Get(id);
            return _mapper.Map<SpecialityDto>(result);
        }

        [HttpPost]
        [ServiceFilter(typeof(AuthorizedFilter))]
        public async Task<ActionResult<SpecialityDto>> Post([FromBody] SpecialityCreateDto createDto)
        {
            var exists = _repository.Exists(p => p.Name.ToLower() == createDto.Name.ToLower());
            if (exists) return BadRequest($"Already exists a speciality with the name: {createDto.Name}");
            var entity = _mapper.Map<Speciality>(createDto);
            _repository.Add(entity);
            await _repository.SaveChanges();

            return CreatedAtRoute(nameof(GetById), new { id = entity.Id }, _mapper.Map<SpecialityDto>(entity));
        }

        [HttpPut("{id}")]
        [Authorize(nameof(AuthorizedFilter))]
        public async Task<ActionResult> Put(string id, [FromBody] SpecialityCreateDto createDto)
        {
            var entity = _repository.Get(id);
            if(entity == null) return NotFound($"Could not get a speciality with id: {id}");
            
            var exists = _repository.Exists(p => p.Name.ToLower() == createDto.Name.ToLower() && p.Id != id);
            if (exists) return BadRequest($"Already exists a speciality with the name: {createDto.Name}");
            
            _mapper.Map(createDto, entity);
            _repository.Update(entity);
            await _repository.SaveChanges();

            return NoContent();
        }

        [HttpPut("ActiveOrDisactive/{id}")]
        [Authorize(nameof(AuthorizedFilter))]
        public async  Task<ActionResult> ActiveOrDisactive(string id)
        {
            var exists = _repository.Exists(p => p.Id == id);
            if (!exists) return NotFound($"Could not get a speciality with id: {id}");
            _repository.ActiveOrDisactive(id);
            await _repository.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(nameof(AuthorizedFilter))]
        public async Task<ActionResult> Delete(string id)
        {
            var exists = _repository.Exists(p => p.Id == id);
            if (!exists) return NotFound($"Could not get a speciality with id: {id}");
            _repository.Delete(id);
            _repository.SaveChanges();
            return NoContent();
        }
    }
}
