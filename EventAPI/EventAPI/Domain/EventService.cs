using EventAPI.CustomExceptions;
using EventAPI.Domain.Interfaces;
using EventAPI.DomainModels;
using EventAPI.Entities;
using EventAPI.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Transactions;

namespace EventAPI.Domain
{
    public class EventService : BaseService<Event, EventModel, IEventRepository>, IEventService
    {
        private readonly IEventGuestService _eventGuestService;
        private readonly IGuestService _guestService;
        public EventService(IEventRepository repository, IEventGuestService eventGuestService, IGuestService guestService) 
            : base(repository) 
        {
            _eventGuestService = eventGuestService;
            _guestService = guestService;
        }

        public async Task<EventModel> CreateEventAsync(EventModel model, ICollection<GuestModel> guestsModels)
        {
            ValidateTimeProperties(model);

            //No need to validate emails here since its done when JSON is sent to the server with data attributes

            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var createdModel = await _repository.CreateAsync(model);

                if (guestsModels.Count == 0)
                {
                    scope.Complete();
                    return createdModel;
                }

                List<GuestModel> guestsInEvent = new List<GuestModel>();

                foreach (var guest in guestsModels)
                {
                    var guestEntity = await _guestService.GetGuestAsync(guest);

                    if (guestEntity is null)
                    {
                        var createdGuest = await _guestService.CreateModelAsync(guest);
                        guestsInEvent.Add(createdGuest);
                    }
                    else
                    {
                        guestsInEvent.Add(guestEntity);
                    }
                }

                await _eventGuestService.CreateEventWithGuestsAsync(createdModel.Id, guestsInEvent);

                var record = await _repository.GetAsync(createdModel.Id);

                scope.Complete();

                return record;
            }
        }

        public async Task<EventModel> UpdateEventAndGuestsAsync(EventModel eventModel, ICollection<string> guestsEmails)
        {
            if (await _repository.GetAsync(eventModel.Id) is null)
                return null;

            ValidateTimeProperties(eventModel);

            ValidateEmails(guestsEmails);

            using(TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var updatedEvent = await _repository.UpdateAsync(eventModel.Id, eventModel);

                List<GuestModel> guestsInEvent = new List<GuestModel>();

                //First remove all previously assigned guests to an event
                await _eventGuestService.RemoveAllGuestsForAnEventAsync(updatedEvent.Id);

                if (guestsEmails.Count != 0)
                {
                    foreach (var guestEmail in guestsEmails)
                    {
                        var guestEntity = await _guestService.GetGuestByEmailAsync(guestEmail);

                        //If guest with a specified email does not exist in DB, throw a Custom Exception that it needs to be created first
                        if (guestEntity is null)
                        {
                            throw new GuestDoesNotExistException($"Guest with email {guestEmail} does not exist in database" +
                                $". Please create them first.");
                        }
                        else
                        {
                            guestsInEvent.Add(guestEntity);
                        }
                    }
                    await _eventGuestService.CreateEventWithGuestsAsync(updatedEvent.Id, guestsInEvent);
                }

                scope.Complete();

                return updatedEvent;
            }
        }

        private void ValidateTimeProperties(EventModel model)
        {
            if(model.StartDate < DateTime.Now || model.EndDate < DateTime.Now)
                throw new WrongTimeFormatException("You can't create an event with start or end date before current one.");
            if (model.StartDate > model.EndDate)
                throw new WrongTimeFormatException("Event start date can't be after its end date.");
        }

        private void ValidateEmails(ICollection<string> emails)
        {
            foreach(var email in emails)
            {
                if (!(!string.IsNullOrEmpty(email) && new EmailAddressAttribute().IsValid(email)))
                    throw new InvalidEmailAddressFormatException($"Email {email} is in invalid email format.");
            }
        }
    }
}
