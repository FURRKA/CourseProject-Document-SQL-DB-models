using AutoMapper;
using BLL.DTO;
using DAL.Entities;

namespace BLL.Profiles
{
    internal class TicketProfile : Profile
    {
        public TicketProfile()
        {
            CreateMap<TicketEntity, TicketDTO>().ReverseMap();
        }
    }
}
