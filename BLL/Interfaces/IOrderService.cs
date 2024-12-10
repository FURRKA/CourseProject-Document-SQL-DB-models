namespace BLL.Interfaces
{
    public interface IOrderService
    {
        public void PrintRoutes();
        public void CreateOrder();
        public void DeleteOrder();
        public void PrintStationsInRoute();
    }
}
