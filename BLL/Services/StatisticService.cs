using BLL.DTO;
using BLL.Interfaces;

namespace BLL.Services
{
    internal class StatisticService : IStatisticService
    {
        private readonly IService<OrderDTO> _orderService;
        public StatisticService(IService<OrderDTO> orderServices)
        {
            _orderService = orderServices;
        }
        public void ShowPaymentStatistic()
        {
            throw new NotImplementedException();            
        }

        public void ShowRouteStatistic()
        {
            throw new NotImplementedException();
        }
    }
}
