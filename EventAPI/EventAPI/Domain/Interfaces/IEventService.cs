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
    public interface IEventService : IBaseService<Event, EventModel, IEventRepository>
    {
        Task<EventModel> CreateEventAsync(EventModel model, ICollection<GuestModel> guestModels);
        Task<EventModel> UpdateEventAndGuestsAsync(EventModel eventModel, ICollection<string> guestsEmails);
    }
}
