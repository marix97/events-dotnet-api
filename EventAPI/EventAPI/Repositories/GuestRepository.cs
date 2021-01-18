using AutoMapper;
using EventAPI.DomainModels;
using EventAPI.Entities;
using EventAPI.Entities.DatabaseContext;
using EventAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventAPI.Repositories
{
    public class GuestRepository : BaseRepository<Guest, GuestModel>, IGuestRepository
    {
        public GuestRepository(IMapper mapper, EventsAPIContext context) : base(mapper, context)
        {

        }

        public async Task<GuestModel> GetGuestAsync(GuestModel model)
        {
            var record = await _context.Guests.FirstOrDefaultAsync(g => g.Email == model.Email);

            return _mapper.Map<GuestModel>(record);
        }

        public async Task<GuestModel> GetGuestByEmailAsync(string email)
        {
            var record = await _context.Guests.FirstOrDefaultAsync(g => g.Email == email);

            return _mapper.Map<GuestModel>(record);
        }
    }
}
