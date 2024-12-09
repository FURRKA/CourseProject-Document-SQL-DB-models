using AutoMapper;
using BLL.DTO;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.Services
{
    internal class StationService : Service<StationsEntity, StationDTO>
    {
        public StationService(IRepository<StationsEntity> repository, IMapper mapper) : base(repository, mapper) { }
    }
}
