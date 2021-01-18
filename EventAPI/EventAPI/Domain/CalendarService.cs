using EventAPI.Domain.Interfaces;
using EventAPI.DomainModels;
using EventAPI.Repositories.Interfaces;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAPI.Domain
{
    public class CalendarService : ICalendarService
    {
        private readonly IEventRepository _eventRepository;

        public CalendarService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public byte[] GetCalendarForAnEvent(EventModel eventModel)
        {
            var calendarEvent = new CalendarEvent()
            {
                Start = new CalDateTime(eventModel.StartDate),
                End = new CalDateTime(eventModel.EndDate),
                Summary = eventModel.Name,
            };

            var calendar = new Calendar();

            calendar.Events.Add(calendarEvent);

            return SerializeCalendar(calendar);
        }

        public async Task<byte[]> GetHostCalendarAsync(string hostName)
        {
            var events = await _eventRepository.GetAllEventsForHostAsync(hostName);

            var calendarEvents = events.Select(e => new CalendarEvent()
            {
                Start = new CalDateTime(e.StartDate),
                End = new CalDateTime(e.EndDate),
                Summary = e.Name,
            });

            var calendar = new Calendar();

            foreach (var calendarEvent in calendarEvents)
            {
                calendar.Events.Add(calendarEvent);
            }

            return SerializeCalendar(calendar);
        }

        private byte[] SerializeCalendar(Calendar calendar)
        {
            var serializer = new CalendarSerializer();
            var serializedCalendar = serializer.SerializeToString(calendar);
            var bytes = Encoding.UTF8.GetBytes(serializedCalendar);

            return bytes;
        }
    }
}
