using AutoMapper;
using BLL.DTO;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.Services
{
    public class ClientService : Service<ClientsEntity, ClientDTO>
    {
        public ClientService(IRepository<ClientsEntity> repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
