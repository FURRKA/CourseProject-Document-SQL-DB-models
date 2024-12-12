using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IOrderService
    {
        public void CreateOrder(ClientDTO client);
        public void DeleteOrder(ClientDTO client);
        public void PrintRoutes();
        public void PrintStations();
        public void PayOrders(ClientDTO client);
        public void PrintActiveOrders(ClientDTO client);
        public void PrintStationsInRoute(int idRoute);
    }
}
