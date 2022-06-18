using AutoMapper;
using FoodDeliverySampleApplication.API;
using FoodDeliverySampleApplication.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDeliverySampleApplication.Logic.Helper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Category, CategoryResponse>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Item, ItemResponse>().ReverseMap();
            CreateMap<Item, ItemDTO>().ReverseMap();
        }
    }
}
