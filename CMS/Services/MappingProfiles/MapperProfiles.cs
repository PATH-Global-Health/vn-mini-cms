using AutoMapper;
using Data.MongoCollections;
using Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.MappingProfiles
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            //Category
            CreateMap<Category, CategoryViewModel>().ReverseMap();
            CreateMap<Category, CategoryUpdateModel>().ReverseMap();
            CreateMap<Category, CategoryAddModel>().ReverseMap();

            //Tag
            CreateMap<Tag, TagViewModel>().ReverseMap();
            CreateMap<Tag, TagUpdateModel>().ReverseMap();
            CreateMap<Tag, TagAddModel>().ReverseMap();

            //Part
            CreateMap<Part, PartViewModel>().ReverseMap();
            CreateMap<Part, PartAddModel>().ReverseMap();
            CreateMap<PartUpdateModel, Part>().ForMember(f => f.Id, map => map.Ignore()).ReverseMap();

            //Post
            //CreateMap<Post, PostViewModel>()
            //    .ForMember(f => f.Categories, map => map.Ignore())
            //    .ForMember(f => f.Tags, map => map.Ignore());
            CreateMap<PostViewModel, Post>()
                .ForMember(f => f.Categories, map => map.Ignore())
                .ForMember(f => f.Tags, map => map.Ignore());

            CreateMap<PostAddModel, Post>()
                .ForMember(f => f.Categories, map => map.Ignore())
                .ForMember(f => f.Tags, map => map.Ignore())
                .ReverseMap();
            CreateMap<PostUpdateModel, Post>()
                .ForMember(f => f.Id, map => map.Ignore())
                .ForMember(f => f.Categories, map => map.Ignore())
                .ForMember(f => f.Tags, map => map.Ignore())
                .ReverseMap();
        }
    }
}
