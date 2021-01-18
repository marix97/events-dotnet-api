using EventAPI.Domain.Interfaces;
using EventAPI.DomainModels;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using EventAPI.Helpers;

namespace EventAPI.Domain
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfiguration;
        private readonly MailMessage _message;
        private readonly ICalendarService _calendarService;
        private readonly IEventGuestService _eventGuestService;

        public EmailService(EmailConfiguration emailConfiguration, MailMessage message, IEventGuestService eventGuestService,
            ICalendarService calendarService)
        {
            _emailConfiguration = emailConfiguration;
            _message = message;
            _eventGuestService = eventGuestService;
            _calendarService = calendarService;
        }

        public async Task SendAsync(string from, string[] to, string subject, string body)
        {
                _message.From = new MailAddress(from);

                foreach (var emailRecipient in to)
                {
                    _message.To.Add(emailRecipient);
                }

                _message.Subject = subject;
                _message.Body = body;

                await SendEmailAsync(_message);
        }

        public async Task SendAsync(string from, string to, string subject, string body, string[] cc)
        {
                foreach (var ccMessage in cc)
                {
                    _message.CC.Add(ccMessage);
                }

                await SendAsync(from, new string[1] { to }, subject, body);
        }

        public async Task SendAsync(string from, string to, string subject, string body, string[] cc, string[] bcc)
        {
                foreach (var bccMessage in bcc)
                {
                    _message.Bcc.Add(bccMessage);
                }

                await SendAsync(from, to, subject, body, cc);
        }

        public async Task SendEmailsToAllGuestsWhenEventCreatedAsync(EventModel specifiedEvent)
        {
            var allGuestsInAnEvent = await _eventGuestService.GetAllGuestsForAnEventAsync(specifiedEvent.Id);

            if (allGuestsInAnEvent.Any())
            {
                var emailsOfGuestsInAnEvent = allGuestsInAnEvent.Select(g => g.Email).ToArray();

                byte[] serializedCalendarData = _calendarService.GetCalendarForAnEvent(specifiedEvent);

                MemoryStream ms = new MemoryStream(serializedCalendarData);

                _message.Attachments.Add(AddAttachmentToEmail(ms, "calendar.ics", "text/calendar"));

                await SendAsync(_emailConfiguration.EmailSender, emailsOfGuestsInAnEvent, "New Event",
                    "I am sending you this email with a calendar attached to it " +
                    $"because you are subscribed to an event with name \"{specifiedEvent.Name}\" and host \"{specifiedEvent.Host}\".");
            }
        }
        private Attachment AddAttachmentToEmail(MemoryStream memoryStream, string fileName, string mediaType)
        {
             return new Attachment(memoryStream, fileName, mediaType);
        }

        private async Task SendEmailAsync(MailMessage message)
        {
                using (var smtpClient = new SmtpClient(_emailConfiguration.SmtpServer, _emailConfiguration.Port))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(_emailConfiguration.Username, _emailConfiguration.Password);
                    smtpClient.EnableSsl = true;
                    await smtpClient.SendMailAsync(message);
                }
        }
    }
}
