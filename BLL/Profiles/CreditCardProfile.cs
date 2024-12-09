using AutoMapper;
using DAL.Entities;

namespace BLL.Profiles
{
    internal class CreditCardProfile : Profile
    {
        public CreditCardProfile()
        {
            CreateMap<CreditsCard, CreditCardProfile>().ReverseMap();
        }
    }
}
