using AutoMapper;
using BLL.Services;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.DTO
{
    internal class TicketOrdersService : Service<TicketOrdersEntity, TicketOrdersDTO>
    {
        public TicketOrdersService(IRepository<TicketOrdersEntity> repository, IMapper mapper) : base(repository, mapper) { }
    }
}
