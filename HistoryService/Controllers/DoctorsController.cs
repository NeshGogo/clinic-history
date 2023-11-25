using Microsoft.AspNetCore.Mvc;
using HistoryService.Data;
using HistoryService.Entities;
using AutoMapper;
using HistoryService.Dtos;

namespace HistoryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IBaseRepo<Doctor> _repo;
        private readonly IMapper _mapper;

        public DoctorsController(IBaseRepo<Doctor> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<DoctorDto>> GetAll()
        {
            var results = _repo.Get();
            return _mapper.Map<List<DoctorDto>>(results);
        }

        [HttpGet("{id}")]
        public ActionResult<DoctorDto> GetById(string id)
        {
            var result = _repo.Get(id);
            return _mapper.Map<DoctorDto>(result);
        }
    }
}
