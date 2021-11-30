using Onion.Application.DataAccess.Entities;
using Onion.Application.DataAccess.Repositories;
using Onion.Application.Services.Abstractions;
using Onion.Application.Services.Exceptions;
using Onion.Application.Services.Models.User;
using Onion.Core.Mapper;
using Onion.Core.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Application.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public UserService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<UserRes> GetAsync(Guid userId)
        {
            var user = await _repositoryManager.UserRepository.GetByIdAsync(userId);
            if (user == null) throw new UserNotFoundException(userId);

            return _mapper.Map<User, UserRes>(user);
        }

        public async Task<IList<UserRes>> ListAsync()
        {
            var users = await _repositoryManager.UserRepository.ListAsync();
            return users.Select(u => _mapper.Map<User, UserRes>(u)).ToList();
        }

        public async Task<UserRes> CreateAsync(UserReq model)
        {
            var newUser = _mapper.Map<UserReq, User>(model, (u =>
            {
                Password.Hash(model.Password, out byte[] hash, out byte[] salt);
                u.PasswordHash = hash;
                u.PasswordSalt = salt;
            }));
            var user = await _repositoryManager.UserRepository.CreateAsync(newUser);

            return _mapper.Map<User, UserRes>(user);
        }
    }
}
