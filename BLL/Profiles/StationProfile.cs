using AutoMapper;
using BLL.DTO;
using DAL.Entities;

namespace BLL.Profiles
{
    internal class StationProfile : Profile
    {
        public StationProfile()
        {
            CreateMap<StationsEntity, StationDTO>().ReverseMap();
        }
    }
}
