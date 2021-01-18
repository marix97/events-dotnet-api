using AutoMapper;
using EventAPI.CreateResponseModels.CreateModels;
using EventAPI.CreateResponseModels.ResponseModels;
using EventAPI.DomainModels;
using EventAPI.Entities;

namespace EventAPI.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Event, EventModel>();
            CreateMap<EventModel, Event>();
            CreateMap<CreateEventModel, EventModel>();
            CreateMap<EventModel, CreateEventModel>();
            CreateMap<EventModel, EventResponseModel>();
            CreateMap<EventResponseModel, EventModel>();
            CreateMap<UpdateEventModel, EventModel>();
            CreateMap<EventModel, UpdateEventModel>();

            CreateMap<Guest, GuestModel>();
            CreateMap<GuestModel, Guest>();
            CreateMap<CreateGuestModel, GuestModel>();
            CreateMap<GuestModel, CreateGuestModel>();
            CreateMap<GuestModel, GuestResponseModel>();
            CreateMap<GuestResponseModel, GuestModel>();

            CreateMap<EventGuest, EventGuestModel>()
                .ForMember(x => x.Event, opt => opt.Ignore())
                .ForMember(x => x.Guest, opt => opt.Ignore());
            CreateMap<EventGuestModel, EventGuest>();
        }
    }
}
