using AutoMapper;
using HistoryService.Data;
using HistoryService.Dtos;
using HistoryService.Entities;
using HistoryService.Helppers;
using Microsoft.AspNetCore.Mvc;

namespace HistoryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IBaseRepo<Patient> _repo;
        private readonly IMapper _mapper;

        public PatientsController(IBaseRepo<Patient> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        [TypeFilter(typeof(AuthorizedFilter))]
        public ActionResult<IEnumerable<PatientDto>> GetAll()
        {
            var results = _repo.Get();
            return _mapper.Map<List<PatientDto>>(results);
        }


        [HttpGet("{id}", Name = "GetPatientById")]
        [TypeFilter(typeof(AuthorizedFilter))]
        public ActionResult<PatientDto> GetById(string id)
        {
            var result = _repo.Get(id);
            return _mapper.Map<PatientDto>(result);
        }

        [HttpGet("exists/{identification}")]
        [TypeFilter(typeof(AuthorizedFilter))]
        public ActionResult<bool> Exists(string identification)
        {
            return _repo.Exists(p => p.Identification == identification);
        }

        [HttpPost]
        [TypeFilter(typeof(AuthorizedFilter))]
        public async Task<ActionResult<PatientDto>> Post([FromBody] PatientCreateDto createDto)
        {
            var exists = _repo.Exists(p => p.Identification == createDto.Identification);
            if (exists) return BadRequest($"Already exists a patient with the identification: {createDto.Identification}");
            var entity = _mapper.Map<Patient>(createDto);
            _repo.Add(entity);
            await _repo.SaveChanges();
            return CreatedAtRoute("GetPatientById", new { id = entity.Id }, _mapper.Map<PatientDto>(entity));
        }
    }
}
