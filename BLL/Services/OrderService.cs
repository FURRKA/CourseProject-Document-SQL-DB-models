using AutoMapper;
using BLL.DTO;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.Services
{
    internal class OrderService : Service<OrderEntity, OrderDTO>
    {
        public OrderService(IRepository<OrderEntity> repository, IMapper mapper) : base(repository, mapper) { }
    }
}
