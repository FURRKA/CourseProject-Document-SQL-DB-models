using AutoMapper;
using BLL.DTO;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.Services
{
    internal class TicketService : Service<TicketEntity, TicketDTO>
    {
        public TicketService(IRepository<TicketEntity> repository, IMapper mapper) : base(repository, mapper) { }
    }
}
