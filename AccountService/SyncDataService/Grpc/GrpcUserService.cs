using AccountService.Data.Repositories;
using AutoMapper;
using Grpc.Core;
using System.Collections.Generic;

namespace AccountService.SyncDataService.Grpc
{
    public class GrpcUserService : GrpcUser.GrpcUserBase
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public GrpcUserService(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public override Task<UserResponse> GetAllUsers(GetAllRequest request, ServerCallContext callContext)
        {
            var response = new UserResponse();
            var users = _repository.GetAll();
            var usersMapped = _mapper.Map<List<GrpcUserModel>>(users);
            response.User.AddRange(usersMapped);
            return Task.FromResult(response);
        }
    }
}
