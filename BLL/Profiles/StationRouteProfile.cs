using AutoMapper;
using BLL.DTO;
using DAL.Entities;

namespace BLL.Profiles
{
    internal class StationRouteProfile : Profile
    {
        public StationRouteProfile()
        {
            CreateMap<StationRoutesEntity, StationRoutesDTO>().ReverseMap();
        }
    }
}
