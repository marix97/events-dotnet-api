using EventAPI.Domain.Interfaces;
using EventAPI.DomainModels;
using EventAPI.Entities;
using EventAPI.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventAPI.Domain
{
    public class GuestService : BaseService<Guest, GuestModel, IGuestRepository>, IGuestService
    {
        public GuestService(IGuestRepository repository) : base(repository) { }

        public async Task<GuestModel> GetGuestAsync(GuestModel model)
        {
            return await _repository.GetGuestAsync(model);
        }

        public async Task<GuestModel> GetGuestByEmailAsync(string email)
        {
            return await _repository.GetGuestByEmailAsync(email);
        }
    }
}
