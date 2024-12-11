using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IOrderService
    {
        public void CreateOrder(ClientDTO client);
        public void DeleteOrder(ClientDTO client);
        public void PrintRoutes();
        public void PrintStationsInRoute(int idRoute);
    }
}
