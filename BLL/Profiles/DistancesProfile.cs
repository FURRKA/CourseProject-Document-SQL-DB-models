using AutoMapper;
using BLL.DTO;
using DAL.Entities;

namespace BLL.Profiles
{
    internal class DistancesProfile : Profile
    {
        public DistancesProfile()
        {
            CreateMap<Distances, DistancesDTO>().ReverseMap();
        }
    }
}
