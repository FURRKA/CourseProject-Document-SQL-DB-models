using AutoMapper;
using BLL.DTO;
using DAL.Entities;

namespace BLL.Profiles
{
    internal class CreditCardProfile : Profile
    {
        public CreditCardProfile()
        {
            CreateMap<CreditsCard, CreditCardDTO>().ReverseMap();
        }
    }
}
