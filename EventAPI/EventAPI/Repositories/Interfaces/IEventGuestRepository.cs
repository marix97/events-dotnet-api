using EventAPI.DomainModels;
using EventAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventAPI.Repositories.Interfaces
{
    public interface IEventGuestRepository : IBaseRepository<EventGuest, EventGuestModel>
    {
        Task<ICollection<GuestModel>> GetGuestsForAnEventAsync(int eventId);
        Task<ICollection<EventModel>> GetAllEventsForAGuestAsync(int guestId);
        Task<EventGuestModel> UpdateEventWithGuestsAsync(EventGuestModel model);
        Task RemoveAllGuestsAsync(int eventId);
    }
}
