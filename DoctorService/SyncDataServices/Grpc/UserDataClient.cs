using AccountService;
using AutoMapper;
using DoctorService.Entities;
using Grpc.Net.Client;

namespace DoctorService.SyncDataServices.Grpc
{
    public class UserDataClient : IUserDataClient
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly ILogger<UserDataClient> _logger;

        public UserDataClient(ILogger<UserDataClient> logger, IConfiguration configuration, IMapper mapper)
        {
            _config = configuration;
            _mapper = mapper;
            _logger = logger;
        }

        public IEnumerable<User> ReturnsAllUsers()
        {
            var host = _config["Services-host:GrpcAccountService"];
            _logger.LogInformation($"--> Calling GRPC Service {host}");
            var channel = GrpcChannel.ForAddress(host);
            var client = new GrpcUser.GrpcUserClient(channel);
            var request = new AccountService.GetAllRequest();

            try
            {
                var reply = client.GetAllUsers(request);
                return _mapper.Map<IEnumerable<User>>(reply.User);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Could not call GRPC Server: {ex.Message}");
                return null;
            }
        }
    }
}
