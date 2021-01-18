using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventAPI.DomainModels
{
    public class EventGuestModel : BaseModel
    {
        public int EventId { get; set; }
        public EventModel Event { get; set; }
        public int GuestId { get; set; }
        public GuestModel Guest { get; set; }
    }
}
