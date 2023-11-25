using AutoMapper;
using DoctorService;
using Grpc.Net.Client;
using HistoryService.Entities;

namespace HistoryService.SyncDataServices.Grpc
{
    public class DoctorDataClient : IDoctorDataClient
    {
        private readonly ILogger<DoctorDataClient> _logger;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public DoctorDataClient(ILogger<DoctorDataClient> logger, IConfiguration configuration, IMapper mapper)
        {
            _logger = logger;
            _config = configuration;
            _mapper = mapper;   
        }

        public IEnumerable<Doctor> ReturnsAllDoctors()
        {
            var host = _config["Services-host:GrpcDoctorService"];
            _logger.LogInformation($"--> Calling GRPC Service {host}");
            var channel = GrpcChannel.ForAddress(host);
            var client = new GrpcDoctor.GrpcDoctorClient(channel);
            var request = new DoctorService.GetAllRequest();

            try
            {
                var reply = client.GetAllDoctors(request);
                return _mapper.Map<IEnumerable<Doctor>>(reply.Doctor);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Could not call GRPC Server when calling {host}: {ex.Message}");
                return null;
            }
        }
    }
}
