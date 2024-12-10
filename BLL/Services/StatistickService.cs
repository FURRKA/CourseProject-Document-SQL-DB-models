using BLL.DTO;
using BLL.Interfaces;

namespace BLL.Services
{
    internal class StatistickService : IStatisticService
    {
        private readonly IService<OrderDTO> _orderService;
        public StatistickService(IService<OrderDTO> orderServices)
        {
            _orderService = orderServices;
        }
        public void ShowPaymentStatistic()
        {
            
        }

        public void ShowRouteStatistic()
        {
            throw new NotImplementedException();
        }
    }
}
