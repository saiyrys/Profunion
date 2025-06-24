using AutoMapper;
using profunion.Domain.Models.EventModels;
using profunion.Shared.Dto.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Applications.Mapper
{
    public class MappingCategory : Profile
    {
        public MappingCategory()
        {
            CreateMap<CategoriesDto, Categories>();

            CreateMap<Categories, CategoriesDto>();

            CreateMap<CreateCategoriesDto, Categories>();

            CreateMap<Categories, CreateCategoriesDto>();

        }
    }
}
