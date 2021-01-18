using AutoMapper;
using EventAPI.APIAuthorization;
using EventAPI.CreateResponseModels.CreateModels;
using EventAPI.CreateResponseModels.ResponseModels;
using EventAPI.CustomExceptions;
using EventAPI.Domain.Interfaces;
using EventAPI.DomainModels;
using EventAPI.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKeyAuthorization]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IEventGuestService _eventGuestService;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public EventsController(IEventService eventService, IEventGuestService eventGuestService, IMapper mapper, IEmailService emailService)
        {
            _eventService = eventService;
            _eventGuestService = eventGuestService;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<IActionResult> Get([FromQuery] PaginationParameters parameters)
        {
            var paginationResults = await _eventService.GetWithPaginationAsync(parameters);

            if (paginationResults.Count == 0)
                return NotFound("Results not found.");

            var responseModels = _mapper.Map<ICollection<EventResponseModel>>(paginationResults);

            foreach(var response in responseModels)
            {
                var guestsForPaginatedRecord = await _eventGuestService.GetAllGuestsForAnEventAsync(response.Id);
                response.Guests = _mapper.Map<ICollection<GuestResponseModel>>(guestsForPaginatedRecord);
            }

            return Ok(responseModels);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var eventModel = await _eventService.GetModelAsync(id);

            if (eventModel is null)
                return NotFound();

            var guestsForCreatedRecord = await _eventGuestService.GetAllGuestsForAnEventAsync(eventModel.Id);

            var response = _mapper.Map<EventResponseModel>(eventModel);
            response.Guests = _mapper.Map<ICollection<GuestResponseModel>>(guestsForCreatedRecord);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateEventModel eventModel)
        {
            try
            {
                var eventToCreate = _mapper.Map<EventModel>(eventModel);
                var guestsToCreate = _mapper.Map<ICollection<GuestModel>>(eventModel.Guests);

                var createRecord = await _eventService.CreateEventAsync(eventToCreate, guestsToCreate);

                if (createRecord is null)
                    return BadRequest("Something went wrong.");

                var guestsForCreatedRecord = await _eventGuestService.GetAllGuestsForAnEventAsync(createRecord.Id);

                var response = _mapper.Map<EventResponseModel>(createRecord);
                response.Guests = _mapper.Map<ICollection<GuestResponseModel>>(guestsForCreatedRecord);

                await _emailService.SendEmailsToAllGuestsWhenEventCreatedAsync(createRecord);

                return Created($"/{response.Id}", response);
            }
            catch (WrongTimeFormatException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var record = await _eventService.DeleteModelAsync(id);

            if (record == default)
                return NotFound($"Record with id {id} not found.");

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UpdateEventModel updateModel)
        {
            try
            {
                var record = _mapper.Map<EventModel>(updateModel);
                record.Id = id;

                var updatedModel = await _eventService.UpdateEventAndGuestsAsync(record, updateModel.GuestsEmails);

                if (updatedModel is null)
                    return NotFound($"Record with id {id} not found.");

                var guestsForUpdatedRecord = await _eventGuestService.GetAllGuestsForAnEventAsync(updatedModel.Id);
                var response = _mapper.Map<EventResponseModel>(updatedModel);
                response.Guests = _mapper.Map<ICollection<GuestResponseModel>>(guestsForUpdatedRecord);

                return Ok(response);
            }
            catch(InvalidEmailAddressFormatException e)
            {
                return BadRequest(e.Message);
            }
            catch (WrongTimeFormatException e)
            {
                return BadRequest(e.Message);
            }
            catch (GuestDoesNotExistException e)
            {
                return BadRequest($"{e.Message}");
            }
            catch (UniqueEmailException)
            {
                return BadRequest("User with that email address already exists.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
