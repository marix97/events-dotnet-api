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
    public class GuestsController : ControllerBase
    {
        private readonly IGuestService _guestService;
        private readonly IEventGuestService _eventGuestService;
        private readonly IMapper _mapper;

        public GuestsController(IGuestService guestService, IEventGuestService emailGuestService, IMapper mapper)
        {
            _guestService = guestService;
            _eventGuestService = emailGuestService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Get([FromQuery] PaginationParameters parameters)
        {
            var paginationResults = await _guestService.GetWithPaginationAsync(parameters);

            if (paginationResults.Count == 0)
                return NotFound("Results not found.");

            var responseModels = _mapper.Map<ICollection<GuestResponseModel>>(paginationResults);

            foreach (var response in responseModels)
            {
                var guestsForPaginatedRecord = await _eventGuestService.GetAllEventsForAGuestAsync(response.Id);
                response.Events = _mapper.Map<ICollection<EventResponseModel>>(guestsForPaginatedRecord);
            }

            return Ok(responseModels);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var guest = await _guestService.GetModelAsync(id);

            if (guest == default)
                return NotFound();

            var guestEvents = await _eventGuestService.GetAllEventsForAGuestAsync(guest.Id);

            var response = _mapper.Map<GuestResponseModel>(guest);
            response.Events = _mapper.Map<ICollection<EventResponseModel>>(guestEvents);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateGuestModel guest)
        {
            try
            {
                var record = await _guestService.CreateModelAsync(_mapper.Map<GuestModel>(guest));

                var response = _mapper.Map<GuestResponseModel>(record);

                return Created($"/{response.Id}", response);
            }
            catch(UniqueEmailException)
            {
                return BadRequest("User with that email address already exists.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var record = await _guestService.DeleteModelAsync(id);

            if (record == default)
                return NotFound($"Record with id {id} not found.");

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CreateGuestModel guest)
        {
            try
            {
                var updateRecord = _mapper.Map<GuestModel>(guest);
                updateRecord.Id = id;

                var record = await _guestService.UpdateModelAsync(id, updateRecord);

                if (record is null)
                    return NotFound("Record with this id is not found in the database.");

                var response = await _guestService.GetModelAsync(id);

                return Ok(_mapper.Map<GuestResponseModel>(response));
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
