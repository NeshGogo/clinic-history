using AutoMapper;
using HistoryService.Data;
using HistoryService.Dtos;
using HistoryService.Entities;
using HistoryService.Helppers;
using Microsoft.AspNetCore.Mvc;

namespace HistoryService.Controllers
{
    [Route("api/Patients/{patientId}/[controller]")]
    [ServiceFilter(typeof(PatientExistsFilter))]
    [TypeFilter(typeof(AuthorizedFilter))]
    [ApiController]
    public class ClinicRecordsController : ControllerBase
    {
        private readonly IBaseRepo<ClinicRecord> _repo;
        private readonly IMapper _mapper;

        public ClinicRecordsController(IBaseRepo<ClinicRecord> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ClinicRecordDto>> GetAll(string patientId)
        {
            var results = _repo.Get().Where(p => p.PatientId == patientId);
            return _mapper.Map<List<ClinicRecordDto>>(results);
        }

        [HttpGet("{id}", Name = "GetClinicRecordById")]
        public ActionResult<IEnumerable<ClinicRecordDto>> GetById(string patientId, string Id)
        {
            var results = _repo.Get().Where(p => p.PatientId == patientId);
            return _mapper.Map<List<ClinicRecordDto>>(results);
        }

        [HttpPost]
        public async Task<ActionResult<ClinicRecordDto>> Post(string patientId, [FromBody] ClinicRecordCreateDto createDto)
        {
            var entity = _mapper.Map<ClinicRecord>(createDto);
            _repo.Add(entity);
            await _repo.SaveChanges();
            return CreatedAtRoute("GetClinicRecordById", new { patientId, id = entity.Id }, _mapper.Map<ClinicRecordDto>(entity));
        }
    }
}
