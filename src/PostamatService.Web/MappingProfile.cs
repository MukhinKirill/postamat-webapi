using System.Linq;
using AutoMapper;
using PostamatService.Data.Models;
using PostamatService.Web.DTO;

namespace PostamatService.Web
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Postamat, PostamatDto>();
            CreateMap<OrderForUpdateDto, Order>()
                .ForMember(_=>_.Products, opt=> 
                        opt.Ignore());
            CreateMap<OrderForCreateDto, Order>()
                .ForMember(_ => _.Products, opt =>
                    opt.Ignore())
                .ForMember(_ => _.Status, opt =>
                    opt.MapFrom(_ => OrderStatus.Registered));
            CreateMap<Order, OrderDto>()
                .ForMember(_ => _.PostamatNumber,
                    opt =>
                        opt.MapFrom(_ => _.Postamat.Number))
                .ForMember(_ => _.Products,
                    opt =>
                        opt.MapFrom(_ => _.Products.Select(_ => _.Name)));
        }
    }
}
