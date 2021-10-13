using AutoMapper;
using PostamatService.Data.Models;
using PostamatService.Web.DTO;

namespace PostamatService.Web
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<Postamat, PostamatDto>();
        } 
    }
}
