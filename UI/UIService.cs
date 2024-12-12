using BLL.DTO;
using BLL.Interfaces;

namespace UI
{
    public class UIService
    {
        private readonly IAutorizationService _autorizationService;
        private readonly IOrderService _orderService;
        private readonly IStatisticService _statisticService;
        public UIService(IAutorizationService autorizationService, IOrderService orderService, IStatisticService statisticService)
        {
            _autorizationService = autorizationService;
            _orderService = orderService;
            _statisticService = statisticService;
        }

        public void Run()
        {
            while (true)
            {
                var user = Autorization();
                PrintSelector(user);
            }
        }

        private ClientDTO Autorization()
        {
            Console.Clear();
            do
            {
                Console.Write("Введите логин: ");
                string login = Console.ReadLine();
                Console.Write("Введите пароль: ");
                string password = Console.ReadLine();

                var client = _autorizationService.Autorization(login, password);
                if (client != null)
                {
                    Console.WriteLine($"\n{client.Name}, добро пожаловать в систему!");
                    Console.WriteLine("Нажмите любую клавишу...");
                    Console.ReadKey();
                    return client;
                }
                else
                {
                    Console.WriteLine("Пользователя с таким логином или паролем нет в системе. Попробуйте ещё раз");
                }    
            } while (true);            
        }

        private void PrintSelector(ClientDTO client)
        {
            bool worked = true;
            string data = $"{client.Name}\n" +
                $"1. Посмотреть список маршрутов\n" +
                $"2. Посмотреть список станций\n" +
                $"3. Посмотреть активные заказы\n" +
                $"4. Создать новый заказ\n" +
                $"5. Отменить заказ\n" +
                $"6. Оплатить заказы\n" +
                $"7. Выйти из системы\n";

            while (worked)
            {
                Console.Clear();
                if (client.Role == "Analitic")
                {
                    var newData = data + "8. Посмотреть статистику заработков компании за период\n" +
                        "9. Посмотреть статистику рейсов\n";
                    Console.WriteLine(newData);
                }
                else
                    Console.WriteLine(data);

                Console.Write("Ваш выбор: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        _orderService.PrintRoutes();
                        Console.ReadKey();

                        break;
                    case "2":
                        _orderService.PrintStations();
                        Console.ReadKey();

                        break;
                    case "3":
                        _orderService.PrintActiveOrders(client);
                        Console.ReadKey();

                        break;
                    case "4":
                        _orderService.CreateOrder(client);
                        break;
                    case "5":
                        _orderService.DeleteOrder(client);
                        break;
                    case "6":
                        _orderService.PayOrders(client);
                        Console.ReadKey();
                        break;
                    case "7":
                        worked = false;
                        break;
                    case "8":
                        if (client.Role == "Analitic")
                        {
                            Console.WriteLine("Введите начальную дату");
                            DateTime.TryParse(Console.ReadLine(), out DateTime date1);
                            Console.WriteLine("Введите конечную дату");
                            DateTime.TryParse(Console.ReadLine(), out DateTime date2);

                            if (date1 != null && date2 != null)
                            {
                                _statisticService.ShowPaymentStatistic(date1, date2);
                            }
                            else
                                Console.WriteLine("Одна из дат является не корректной");
                        }
                        break;
                    case "9":
                        if (client.Role == "Analitic")
                            break;
                        break;
                    default:
                        break;
                }
            }
        }

    }
}
