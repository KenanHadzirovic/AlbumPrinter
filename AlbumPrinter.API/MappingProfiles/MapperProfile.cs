using AlbumPrinter.Dto;
using AlbumPrinter.Models;
using AutoMapper;
using System.Diagnostics.CodeAnalysis;

namespace AlbumPrinter.API.MappingProfiles
{
    /// <summary>
    /// Global automapping profile definition
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<OrderProductType, OrderProductTypeDto>()
                .ForMember(x => x.ProductType, y => y.MapFrom(z => z.ProductType.ProductTypeName));
        }
    }
}
