using AutoMapper;
using BLL.DTO;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.Services
{
    internal class ClientService : Service<ClientsEntity, ClientDTO>
    {
        public ClientService(IRepository<ClientsEntity> repository, IMapper mapper) : base(repository, mapper)
        {}
    }
}
