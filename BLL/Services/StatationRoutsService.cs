using AutoMapper;
using BLL.DTO;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.Services
{
    internal class StatationRoutsService : Service<StationRoutesEntity, StationRoutesDTO>
    {
        public StatationRoutsService(IRepository<StationRoutesEntity> repository, IMapper mapper) : base(repository, mapper) { }
    }
}
