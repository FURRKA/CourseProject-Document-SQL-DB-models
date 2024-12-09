using AutoMapper;
using BLL.DTO;
using DAL.Entities;

namespace BLL.Profiles
{
    internal class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<ClientsEntity, ClientDTO>().ReverseMap();
        }
    }
}
