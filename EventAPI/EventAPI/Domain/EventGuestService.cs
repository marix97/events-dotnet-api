using EventAPI.Domain.Interfaces;
using EventAPI.DomainModels;
using EventAPI.Entities;
using EventAPI.Repositories;
using EventAPI.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventAPI.Domain
{
    public class EventGuestService : BaseService<EventGuest, EventGuestModel, IEventGuestRepository>, IEventGuestService
    {
        public EventGuestService(IEventGuestRepository repository) : base(repository) { }

        public async Task CreateEventWithGuestsAsync(int eventId, ICollection<GuestModel> guests)
        {
            foreach (var guest in guests)
            {
                var eventGuest = new EventGuestModel
                {
                    EventId = eventId,
                    GuestId = guest.Id
                };

                await _repository.CreateAsync(eventGuest);
            }
        }

        public async Task<ICollection<EventModel>> GetAllEventsForAGuestAsync(int guestId)
        {
            return await _repository.GetAllEventsForAGuestAsync(guestId);
        }

        public async Task<ICollection<GuestModel>> GetAllGuestsForAnEventAsync(int eventId)
        {
            return await _repository.GetGuestsForAnEventAsync(eventId);
        }

        public async Task RemoveAllGuestsForAnEventAsync(int eventId)
        {
            await _repository.RemoveAllGuestsAsync(eventId);
        }

        public async Task UpdateEventWithGuestsAsync(int eventId, ICollection<GuestModel> guests)
        {
            foreach (var guest in guests)
            {
                var eventGuest = new EventGuestModel
                {
                    EventId = eventId,
                    GuestId = guest.Id
                };

                await _repository.UpdateEventWithGuestsAsync(eventGuest);
            }
        }
    }
}
