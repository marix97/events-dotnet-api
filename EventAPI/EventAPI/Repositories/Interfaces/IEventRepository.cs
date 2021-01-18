using EventAPI.DomainModels;
using EventAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventAPI.Repositories.Interfaces
{
    public interface IEventRepository : IBaseRepository<Event, EventModel>
    {
        Task<ICollection<EventModel>> GetAllEventsForHostAsync(string hostName);
    }
}
