using AutoMapper;

using profunion.Domain.Models.NewsModels;
using profunion.Shared.Dto.News;


namespace profunion.Applications.Mapper
{
    public class MappingNews : Profile
    {
        public MappingNews()
        {
            CreateMap<News, GetNewsDto>();
            CreateMap<GetNewsDto, News>();

            CreateMap<CreateNewsDto, News>();
            CreateMap<News, CreateNewsDto>();

            CreateMap<UpdateNewsDto, News>();
            CreateMap<News, UpdateNewsDto>();
        }
    }
}
