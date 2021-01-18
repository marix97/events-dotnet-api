using EventAPI.DomainModels;
using EventAPI.Entities;
using EventAPI.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventAPI.Domain.Interfaces
{
    public interface IEventGuestService : IBaseService<EventGuest, EventGuestModel, IEventGuestRepository>
    {
        Task CreateEventWithGuestsAsync(int eventId, ICollection<GuestModel> guests);
        Task<ICollection<GuestModel>> GetAllGuestsForAnEventAsync(int eventId);
        Task<ICollection<EventModel>> GetAllEventsForAGuestAsync(int guestId);
        Task UpdateEventWithGuestsAsync(int eventId, ICollection<GuestModel> guests);
        Task RemoveAllGuestsForAnEventAsync(int eventId);
    }
}
