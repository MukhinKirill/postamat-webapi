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
                    opt.Ignore());
            CreateMap<Order, OrderDto>()
                .ForMember(_ => _.Products,
                    opt =>
                        opt.MapFrom(_ => _.Products.Select(p => p.Name)));
        }
    }
}
