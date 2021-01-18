using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventAPI.CreateResponseModels.ResponseModels
{
    public class EventResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Host { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<GuestResponseModel> Guests { get; set; }
    }
}
