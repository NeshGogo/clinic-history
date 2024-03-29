﻿using AutoMapper;
using DoctorService.AsyncDataService;
using DoctorService.Data;
using DoctorService.Data.Repositories;
using DoctorService.Dtos;
using DoctorService.Entities;
using DoctorService.Enums;
using DoctorService.Helppers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IBaseRepository<Doctor> _repository;
        private readonly IMapper _mapper;
        private readonly IMessageBusClient _messageBusClient;
        private readonly IBaseRepository<Speciality> _specialityRepository;

        public DoctorsController(
            IBaseRepository<Doctor> repository, 
            IMapper mapper, 
            IMessageBusClient messageBusClient, 
            IBaseRepository<Speciality> specialityRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _messageBusClient = messageBusClient;
            _specialityRepository = specialityRepository;
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
        [TypeFilter(typeof(AuthorizedFilter))]
        public async Task<ActionResult<DoctorDto>> Post([FromBody] DoctorCreateDto createDto)
        {
            var exists = _repository.Exists(p => p.Identification.ToLower() == createDto.Identification.ToLower());
            if (exists) return BadRequest($"Already exists a doctor with the identification: {createDto.Identification}");
            var entity = _mapper.Map<Doctor>(createDto);
            _repository.Add(entity);
            await _repository.SaveChanges();

            var doctorPublish = _mapper.Map<DoctorPublishDto>(entity);
            doctorPublish.Event = EventType.NewDoctor;
            doctorPublish.Speciality = _specialityRepository.Get(entity.SpecialityId).Name;
            _messageBusClient.PublishNewDoctor(doctorPublish);

            return CreatedAtRoute("GetDoctorById", new { id = entity.Id }, _mapper.Map<DoctorDto>(entity));
        }

        [HttpPut("{id}")]
        [TypeFilter(typeof(AuthorizedFilter))]
        public async Task<ActionResult> Put(string id, [FromBody] DoctorCreateDto createDto)
        {
            var entity = _repository.Get(id);
            if (entity == null) return NotFound($"Could not get a doctor with id: {id}");

            var exists = _repository.Exists(p => p.Identification.ToLower() == createDto.Identification.ToLower() && p.Id != id);
            if (exists) return BadRequest($"Already exists a doctor with the identification:  {createDto.Identification}");

            _mapper.Map(createDto, entity);
            _repository.Update(entity);
            await _repository.SaveChanges();

            return NoContent();
        }

        [HttpPut("ActiveOrDisactive/{id}")]
        [TypeFilter(typeof(AuthorizedFilter))]
        public async  Task<ActionResult> ActiveOrDisactive(string id)
        {
            var exists = _repository.Exists(p => p.Id == id);
            if (!exists) return NotFound($"Could not get a doctor with id: {id}");
            _repository.ActiveOrDisactive(id);
            await _repository.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [TypeFilter(typeof(AuthorizedFilter))]
        public async Task<ActionResult> Delete(string id)
        {
            var exists = _repository.Exists(p => p.Id == id);
            if (!exists) return NotFound($"Could not get a doctor with id: {id}");
            _repository.Delete(id);
            await _repository.SaveChanges();
            return NoContent();
        }
    }
}
