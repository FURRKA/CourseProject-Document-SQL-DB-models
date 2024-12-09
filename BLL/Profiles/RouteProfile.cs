using AutoMapper;
using BLL.DTO;
using DAL.Entities;

namespace BLL.Profiles
{
    internal class RouteProfile : Profile
    {
        public RouteProfile()
        {
            CreateMap<RouteEntity, RouteDTO>().ReverseMap();
        }
    }
}
