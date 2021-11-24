using Onion.Application.DataAccess.Entities;
using Onion.Application.DataAccess.Repositories;
using Onion.Application.Services.Abstractions;
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
        public UserService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<bool> FooAsync()
        {
            var userRepository = _repositoryManager.UserRepository;

            Password.Hash("heslo123", out byte[] hash, out byte[] salt);
            var newUser = new User()
            {
                Email = "hroudaadam@gmail.com",
                PasswordHash = hash,
                PasswordSalt = salt
            };
            var res = await userRepository.CreateAsync(newUser);
            return true;
        }
    }
}
