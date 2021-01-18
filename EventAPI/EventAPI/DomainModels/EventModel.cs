using System;
using System.Collections.Generic;

namespace EventAPI.DomainModels
{
    public class EventModel : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Host { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
