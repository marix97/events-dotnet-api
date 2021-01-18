using EventAPI.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventAPI.Domain.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(string from, string[] to, string subject, string body);
        Task SendAsync(string from, string to, string subject, string body, string[] cc);
        Task SendAsync(string from, string to, string subject, string body, string[] cc, string[] bcc);
        Task SendEmailsToAllGuestsWhenEventCreatedAsync(EventModel specifiedEvent);
    }
}
