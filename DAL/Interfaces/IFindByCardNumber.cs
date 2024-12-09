using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IFindByCardNumber
    {
        public CreditsCard FindByNumber(string number);
    }
}
