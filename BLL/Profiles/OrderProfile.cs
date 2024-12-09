using AutoMapper;
using BLL.DTO;
using DAL.Entities;

namespace BLL.Profiles
{
    internal class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderEntity, OrderDTO>().ReverseMap();
        }
    }
}
