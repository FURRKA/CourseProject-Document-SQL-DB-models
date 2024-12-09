using AutoMapper;
using BLL.DTO;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.Services
{
    internal class DistanceService : Service<Distances, DistancesDTO>
    {
        public DistanceService(IRepository<Distances> repository, IMapper mapper) : base(repository, mapper) { }
    }
}
