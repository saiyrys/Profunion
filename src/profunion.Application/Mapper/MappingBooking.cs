using AutoMapper;
using profunion.Domain.Models.ApplicationModels;
using profunion.Domain.Models.UserModels;
using profunion.Shared.Dto.Application;
using profunion.Shared.Dto.Category;
using profunion.Shared.Dto.Events;
using profunion.Shared.Dto.Uploads;
using profunion.Shared.Dto.Users;
using System.Buffers.Text;

namespace profunion.Applications.Mapper
{
    public class MappingBooking : Profile
    {
        public MappingBooking()
        {

            CreateMap<Application, CreateApplicationDto>();

            CreateMap<CreateApplicationDto, Application>();


            CreateMap<Application, GetApplicationDto>()
                 .ForMember(dest => dest.user, opt => opt.MapFrom(src => new GetUserInfo
                 {
                     firstName = src.User.firstName,
                     middleName = src.User.middleName,
                     lastName = src.User.lastName
                     // добавь другие необходимые поля
                 }))
                 .ForMember(dest => dest.@event, opt => opt.MapFrom(src => new GetEventInfo
                 {
                     title = src.Event.title
                 }));

            CreateMap<Application, GetUserApplicationDto>()
                 .ForMember(dest => dest.takePlaces, opt => opt.MapFrom(src => src.Places));
        }

    }
}
