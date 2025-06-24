using AutoMapper;
using profunion.Domain.Models.EventModels;
using profunion.Shared.Dto.Events;

namespace profunion.Applications.Mapper
{
    public class MappingEvents : Profile
    {
        public MappingEvents()
        {
            CreateMap<Event, GetEventDto>();
            CreateMap<GetEventDto, Event>();

            CreateMap<string, Categories>()
            .ConstructUsing(src => new Categories { Id = src });

            CreateMap<CreateEventDto, Event>();
            CreateMap<Event, CreateEventDto>();


            CreateMap<UpdateEventDto, Event>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Event, UpdateEventDto>();

            CreateMap<DeleteEventDto, Event>();
            CreateMap<Event, DeleteEventDto>();
        }
    }
}
