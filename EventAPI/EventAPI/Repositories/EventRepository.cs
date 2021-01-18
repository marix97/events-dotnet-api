using AutoMapper;
using EventAPI.DomainModels;
using EventAPI.Entities;
using EventAPI.Entities.DatabaseContext;
using EventAPI.Repositories.Interfaces;
using System.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EventAPI.Repositories
{
    public class EventRepository : BaseRepository<Event, EventModel>, IEventRepository
    {
        public EventRepository(IMapper mapper, EventsAPIContext context) : base(mapper, context)
        {

        }

        public async Task<ICollection<EventModel>> GetAllEventsForHostAsync(string hostName)
        {
            var events = await _context.Events.Where(e => e.Host == hostName).ToListAsync();

            return _mapper.Map<ICollection<EventModel>>(events);
        }
    }
}
