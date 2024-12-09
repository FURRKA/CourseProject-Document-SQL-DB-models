using AutoMapper;
using BLL.DTO;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.Services
{
    internal class StationRoutsService : Service<StationRoutesEntity, StationRoutesDTO>
    {
        public StationRoutsService(IRepository<StationRoutesEntity> repository, IMapper mapper) : base(repository, mapper) { }
    }
}
