using AutoMapper;
using BLL.DTO;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.Services
{
    internal class CreditCardService : Service<CreditsCard, CreditCardDTO>
    {
        public CreditCardService(IRepository<CreditsCard> repository, IMapper mapper) : base(repository, mapper) { }
    }
}
