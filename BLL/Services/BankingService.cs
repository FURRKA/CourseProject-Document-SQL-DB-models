using BLL.DTO;
using BLL.Interfaces;
using DAL.Interfaces;

namespace BLL.Services
{
    internal class BankingService : IBankingService
    {
        private readonly IService<CreditCardDTO> _CardService;
        private readonly IFindByCardNumber _findCardService;
        public BankingService(IService<CreditCardDTO> service, IFindByCardNumber findCardService)
        {
            _CardService = service;
            _findCardService = findCardService;
        }
        public bool AddTransaction(string number, int cvc, double value)
        {
            if (CheckBankAccount(number))
            {
                var card = _findCardService.FindByNumber(number);
                if (card.CardNumber == number && card.CVC == cvc)
                {
                    card.Value += value;
                }
                return true;
            }

            return false;
        }

        public bool CheckBankAccount(string number) => _findCardService.FindByNumber(number) != null;

        public bool CheckBankAccount(string number, int cvc) => _findCardService.FindByNumberCVC(number, cvc) != null;

        public bool RemoveTransaction(string number, int cvc, double value)
        {
            if (CheckBankAccount(number, cvc))
            {
                var card = _findCardService.FindByNumber(number);
                if (card.CardNumber == number && card.CVC == cvc)
                {
                    card.Value -= value;
                }
                return true;
            }

            return false;
        }

        
    }
}
