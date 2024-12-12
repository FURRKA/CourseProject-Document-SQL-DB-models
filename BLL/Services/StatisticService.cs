using BLL.DTO;
using BLL.Interfaces;
using Npgsql;

namespace BLL.Services
{
    internal class StatisticService : IStatisticService
    {
        private readonly IService<OrderDTO> _orderService;
        private readonly IService<RouteDTO> _routeService;
        public StatisticService(IService<OrderDTO> orderServices, IService<RouteDTO> routeService)
        {
            _orderService = orderServices;
            _routeService = routeService;
        }
        public void ShowPaymentStatistic(DateTime date1, DateTime date2)
        {
            var t = _orderService.GetByCriteria(o => o.Status == true);
            var orders = _orderService.GetByCriteria(o => o.Status == true &&
                o.Date >= date1 && o.Date <= date2
            );

            var dict = new Dictionary<(int, int), double>();
            orders.ForEach(o =>
            {
                var key = (o.Date.Month, o.Date.Year);
                double value = orders.Where(o => key.Month == o.Date.Month && key.Year == o.Date.Year).Sum(o => o.Cost);
                if (dict.ContainsKey(key))
                {
                    dict[key] = value;
                }
                else
                    dict.Add(key, value);
            });

            Console.WriteLine($"Статистика доходов за период с {date1.Date} по {date2.Date}");
            TableService.Show(
                dict.Select(d => new { month = d.Key.Item1, year = d.Key.Item2, value = d.Value }).ToList(),
                new string[] { "Месяц", "Год", "Прибыль" },
                d => d.month,
                d => d.year,
                d => d.value
                );

            Console.ReadKey();

        }

        public void ShowRouteStatistic(DateTime date1, DateTime date2)
        {
            var orders = _orderService.GetByCriteria(o => o.Status == true && o.Date >= date1 && o.Date <= date2);
            var dict = new Dictionary<(int, int, RouteDTO), int>();

            _routeService.GetAll().ForEach(r =>
            {
                orders.ForEach(o =>
                {
                    var key = (o.Date.Month, o.Date.Year, r);
                    int count = orders.Where(o => o.Tickets.Any(t => t.Route.Id == r.Id)).Count();
                    if (dict.ContainsKey(key))
                    {
                        dict[key] = count;
                    }
                    else
                        dict.Add(key, count);
                });
            });

            Console.WriteLine($"Статистика доходов за период с {date1.Date} по {date2.Date}");
            TableService.Show(
                dict.Select(d => new { month = d.Key.Item1, year = d.Key.Item2, route = d.Key.Item3, value = d.Value }).ToList(),
                new string[] { "Месяц", "Год", "Маршрут", "Количество" },
                d => d.month,
                d => d.year,
                d => d.route.RouteName,
                d => d.value
                );

            Console.ReadKey();
        }
    }
}
