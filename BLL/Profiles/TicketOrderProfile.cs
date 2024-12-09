using AutoMapper;
using BLL.DTO;
using DAL.Entities;

namespace BLL.Profiles
{
    internal class TicketOrderProfile : Profile
    {
        public TicketOrderProfile()
        {
            CreateMap<TicketOrdersEntity, TicketDTO>().ReverseMap();
        }
    }
}
