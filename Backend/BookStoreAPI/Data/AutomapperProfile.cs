using AutoMapper;
using BookStoreAPI.Data.Entities;
using BookStoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreAPI.Data
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            this.CreateMap<EditorialEntity, EditorialModel>()
                .ReverseMap();

            this.CreateMap<BookModel, BookEntity>()
                .ForMember(ent => ent.Editorial, mod => mod.MapFrom(modSrc => new EditorialEntity() { Id = modSrc.EditorialId }))
                .ReverseMap()
                .ForMember(mod => mod.EditorialId, ent => ent.MapFrom(entSrc => entSrc.Editorial.Id));
        }
    }
}
