using EventAPI.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventAPI.Domain.Interfaces
{
    public interface ICalendarService
    {
        Task<byte[]> GetHostCalendarAsync(string hostName);
        byte[] GetCalendarForAnEvent(EventModel eventModel);
    }
}
