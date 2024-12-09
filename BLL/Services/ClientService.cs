using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ClientService : IService<ClientDTO>
    {
        private readonly IRepository<ClientsEntity> _clientRepository;
        private readonly IMapper _mapper;
        public ClientService(IRepository<ClientsEntity> repository, IMapper mapper)
        {
            _clientRepository = repository;
            _mapper = mapper;
        }
        public void Create(ClientDTO entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(ClientDTO entity)
        {
            throw new NotImplementedException();
        }

        public List<ClientDTO> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<ClientDTO> GetByCriteria(Predicate<ClientDTO> predicate)
        {
            throw new NotImplementedException();
        }

        public ClientDTO GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(ClientDTO entity)
        {
            throw new NotImplementedException();
        }
    }
}
