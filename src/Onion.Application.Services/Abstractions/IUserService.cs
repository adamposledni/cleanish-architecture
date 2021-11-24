using Onion.Application.Services.Models.Item;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Onion.Application.Services.Abstractions
{
    public interface IUserService
    {
        Task<bool> FooAsync();
    }
}
