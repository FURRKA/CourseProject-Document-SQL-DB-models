using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IFindByCardNumber
    {
        public CreditsCard FindByNumber(string number);
        public CreditsCard FindByNumberCVC(string number, int cvc);
    }
}
