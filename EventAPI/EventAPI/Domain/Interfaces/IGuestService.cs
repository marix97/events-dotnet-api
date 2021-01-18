using EventAPI.DomainModels;
using EventAPI.Entities;
using EventAPI.Repositories;
using EventAPI.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventAPI.Domain.Interfaces
{
    public interface IGuestService : IBaseService<Guest, GuestModel, IGuestRepository>
    {
        Task<GuestModel> GetGuestAsync(GuestModel model);
        Task<GuestModel> GetGuestByEmailAsync(string email);
    }
}
