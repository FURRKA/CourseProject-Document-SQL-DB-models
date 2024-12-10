namespace BLL.Interfaces
{
    public interface IBankingService
    {
        public bool CheckBankAccount(string number);
        public bool AddTransaction(string number, int cvc, double value);
        public bool RemoveTransaction(string number, int cvc, double value);

    }
}
