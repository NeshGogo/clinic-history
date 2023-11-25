using AutoMapper;
using AccountService;
using Grpc.Net.Client;
using HistoryService.Entities;

namespace HistoryService.SyncDataServices.Grpc
{
    public class UserDataClient : IUserDataClient
    {
        private readonly ILogger<UserDataClient> _logger;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public UserDataClient(ILogger<UserDataClient> logger, IConfiguration config, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _config = config;
        }

        public IEnumerable<User> ReturnsAllUsers()
        {
            var host = _config["Services-host:GrpcAccountService"];
            _logger.LogInformation($"--> Calling GRPC Service {host}");
            var channel = GrpcChannel.ForAddress(host);
            var client = new GrpcUser.GrpcUserClient(channel);
            var request = new GetAllRequest();

            try
            {
                var reply = client.GetAllUsers(request);
                return _mapper.Map<IEnumerable<User>>(reply.User);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Could not call GRPC Server when calling {host}: {ex.Message}");
                return null;
            }
        }
    }
}
