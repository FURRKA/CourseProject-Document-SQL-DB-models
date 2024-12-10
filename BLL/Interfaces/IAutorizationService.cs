using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IAutorizationService
    {
        public ClientDTO? Autorization(string login, string password);
        public bool Registrating();
        public bool DeleteAccount();
    }
}
