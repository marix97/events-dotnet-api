using AutoMapper;
using EventAPI.CustomExceptions;
using EventAPI.DomainModels;
using EventAPI.Entities;
using EventAPI.Entities.DatabaseContext;
using EventAPI.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventAPI.Repositories
{
    public class EventGuestRepository : BaseRepository<EventGuest, EventGuestModel>, IEventGuestRepository
    {
        public EventGuestRepository(IMapper mapper, EventsAPIContext context) : base(mapper, context)
        {

        }

        public async Task<ICollection<EventModel>> GetAllEventsForAGuestAsync(int guestId)
        {
            return _mapper.Map<ICollection<EventModel>>
                (await _context.EventGuest.Where(e => e.GuestId == guestId)
                .Select(e => e.Event)
                .ToListAsync());
        }

        public async Task<ICollection<GuestModel>> GetGuestsForAnEventAsync(int eventId)
        {
            return _mapper.Map<ICollection<GuestModel>>
                (await _context.EventGuest.Where(e => e.EventId == eventId)
                .Select(g => g.Guest)
                .ToListAsync());
        }

        public async Task RemoveAllGuestsAsync(int eventId)
        {
            var toDeleteElements = await _context.EventGuest.Where(i => i.EventId == eventId).ToListAsync();

            _context.EventGuest.RemoveRange(toDeleteElements);
            await _context.SaveChangesAsync();
        }

        public async Task<EventGuestModel> UpdateEventWithGuestsAsync(EventGuestModel model)
        {
            var recordToUpdate = _mapper.Map<EventGuest>(model);

            try
            {
                _context.Entry(recordToUpdate).CurrentValues.SetValues(model);
                await _context.SaveChangesAsync();

                return _mapper.Map<EventGuestModel>(recordToUpdate);
            }
            catch (DbUpdateException e)
            {
                SqlException innerException = e.InnerException as SqlException;
                if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601))
                {
                    throw new UniqueEmailException();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
