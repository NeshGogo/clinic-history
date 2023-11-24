using AutoMapper;
using DoctorService.Data;
using DoctorService.Entities;
using Grpc.Core;

namespace DoctorService.SyncDataServices.Grpc
{
    public class GrpcDoctorService : GrpcDoctor.GrpcDoctorBase
    {
        private readonly IBaseRepository<Doctor> _repo;
        private readonly IMapper _mapper;

        public GrpcDoctorService(IBaseRepository<Doctor> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public override Task<DoctorResponse> GetAllDoctors(GetAllRequest request, ServerCallContext callContext)
        {
            var response = new DoctorResponse();
            var doctors = _repo.Get();
            var doctorsMapped = _mapper.Map<List<GrpcDoctorModel>>(doctors);
            response.Doctor.AddRange(doctorsMapped);
            return Task.FromResult(response);
        }
    }
}
