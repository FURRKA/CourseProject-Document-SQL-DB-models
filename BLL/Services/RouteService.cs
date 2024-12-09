using AutoMapper;
using BLL.DTO;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.Services
{
    internal class RouteService : Service<RouteEntity, RouteDTO>
    {
        public RouteService(IRepository<RouteEntity> repository, IMapper mapper) : base(repository, mapper) { }
    }
}
