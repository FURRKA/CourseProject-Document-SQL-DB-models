using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using MongoDB.Bson.IO;

namespace BLL.Services
{
    internal class ProcessedOrderService : IOrderService
    {
        private readonly IService<OrderDTO> _orderService;
        private readonly IService<RouteDTO> _routeService;
        private readonly IService<StationDTO> _stationService;
        private readonly IService<TicketDTO> _ticketService;
        private readonly IBankingService _bankingService;
        public ProcessedOrderService(IService<OrderDTO> orderService, IService<RouteDTO> routeService, 
            IService<StationDTO> stationService, IService<TicketDTO> ticketService, IBankingService bankingService)
        {
            _orderService = orderService;
            _routeService = routeService;
            _stationService = stationService;
            _ticketService = ticketService;
            _bankingService = bankingService;
        }
        public void CreateOrder(ClientDTO client)
        {
            var tickets = new List<TicketDTO>();
            ConsoleKey key;

            do
            {
                Console.Clear();
                Console.WriteLine("Регистрация билета");
                tickets.Add(CreateTicket(client, tickets));
                PrintTickets(tickets);
                Console.WriteLine("Хотите добавить ещё один билет в заказ? Y/N");
                key = Console.ReadKey().Key;
                if (key == ConsoleKey.Y)
                    continue;

            } while (key != ConsoleKey.N);

            var order = new OrderDTO
            {
                Id = _orderService.GetMaxNewId(),
                Tickets = tickets,
                Date = DateTime.Now.Date,
                CreditsCard = new CreditCardDTO()
            };

            Console.WriteLine("Ваши билеты:");
            PrintTickets(tickets);
            order.CalculateCost();
            tickets.ForEach(_ticketService.Create);
            _orderService.Create(order);
            Console.WriteLine($"Суммарная стоимость заказа {order.Cost}");


            Console.ReadKey();
            //string cardNumber;
            //int cvc;
            //do
            //{
            //    Console.WriteLine("Введите данные карты");
            //    cardNumber = Console.ReadLine();
            //    if (!_bankingService.CheckBankAccount(cardNumber))
            //    {
            //        Console.WriteLine("Карты с таким счётом номером нет в банковской системе. Попробуйте снова");
            //        continue;
            //    }
            //    Console.WriteLine("Введите cvc код");

            //} while (true);


        }

        public void DeleteOrder(ClientDTO client)
        {
            throw new NotImplementedException();
        }

        private TicketDTO CreateTicket(ClientDTO client, List<TicketDTO> lists)
        {
            string departingStationName, arrivingStationName;
            StationDTO departingStation, arrivingStation;
            RouteDTO route;
            DateTime date;
            int routeId, carNumber, seatNumber;

            //Выбор станций
            PrintStations();
            do
            {
                Console.Write("Выберите станцию отправления: ");
                departingStationName = Console.ReadLine();

                if (!CheckStation(departingStationName))
                    Console.WriteLine("Такой станции нет в списке. Попробуйте ещё раз\n");

            } while (!CheckStation(departingStationName));

            departingStation = _stationService.GetByCriteria(s => s.StationName.ToLower() ==  departingStationName.ToLower())[0];

            do
            {
                Console.Write("Выберите станцию прибытия: ");
                arrivingStationName = Console.ReadLine();
                if (!CheckStation(arrivingStationName))
                    Console.WriteLine("Такой станции нет в списке. Попробуйте ещё раз\n");
                if (departingStationName == arrivingStationName)
                    Console.WriteLine("Станция отправления не должна быть такой же что и прибытия. Попробуйте ещё раз");

            } while (!CheckStation(arrivingStationName) || departingStationName == arrivingStationName);

            arrivingStation = _stationService.GetByCriteria(s => s.StationName.ToLower() == arrivingStationName.ToLower())[0];
            

            //Выбор даты
            TableService.Show(GetDates(), new string[] {"Доступные даты" }, d => d.Date);
            do
            {
                Console.Write("Выберите дату отправления: ");
                date = DateTime.Parse(Console.ReadLine());
                if (!GetDates().Contains(date))
                    Console.WriteLine("Данная дата не доступна для поездки");

            } while (!GetDates().Contains(date));

            date.AddHours(1);

            //Выбор подходящего маршрута
            var routes = _routeService.GetByCriteria(r => r.Stations.Any(s => s.Id == departingStation.Id) && r.Stations.Any(s => s.Id == arrivingStation.Id));
            var rids = routes.Select(r => r.Id).ToList();
            Console.WriteLine("Маршруты подходящие под ваши критерии:");
            TableService.Show(routes,
                new string[] {"Маршрут", "Название"},
                r => r.Id,
                r => r.RouteName
                );

            do
            {
                Console.Write("Выберите номер маршрута: ");
                if (!Int32.TryParse(Console.ReadLine(), out routeId))
                {
                    Console.WriteLine("Вы ввели что-то не так. Попробуйте снова");
                    continue;
                }
                if (!rids.Contains(routeId))
                    Console.WriteLine("Данный маршрут не подходит по вашим критериям\n");

            } while (!rids.Contains(routeId));

            route = _routeService.GetById(routeId);

            var cars = new List<(int, string)>()
            {
                (1, "Сидячий"),
                (2, "Плацкарт"),
                (3, "Плацкарт"),
                (4, "Купэ")
            };
            TableService.Show(cars, new string[] { "Вагон", "Тип" }, p => p.Item1, p => p.Item2);

            do
            {
                Console.Write("Выберите номер вагона: ");
                if (!Int32.TryParse(Console.ReadLine(), out carNumber))
                {
                    Console.WriteLine("Вы ввели что-то не так. Попробуйте снова");
                    continue;
                }    
                if (!Enumerable.Range(1,4).Contains(carNumber))
                    Console.WriteLine("Данного вагона нету в системе\n");

            } while (!Enumerable.Range(1, 4).Contains(carNumber));

            var occupiedSeats = _ticketService.GetByCriteria(t => t.Date == date && t.CarNumber == carNumber && t.Route.Id == routeId).Select(t => t.Id).ToList();
            lists.Where(t => t.Date == date && t.CarNumber == carNumber && t.Route.Id == routeId).ToList().ForEach(s => occupiedSeats.Add(s.SeatNumber));
            Draw(occupiedSeats, 64);
            Console.WriteLine("Зелёным цветом отмечены свободные места");

            do
            {
                Console.Write("Выберите желаемое место: ");
                if (!Int32.TryParse(Console.ReadLine(), out seatNumber))
                {
                    Console.WriteLine("Вы ввели что-то не так. Попробуйте снова");
                    continue;
                }
                if (occupiedSeats.Contains(seatNumber))
                    Console.WriteLine("Данное место уже занято\n");
            } while (occupiedSeats.Contains(seatNumber));

            return new TicketDTO
            {
                Id = _ticketService.GetMaxNewId(),
                DepartingStation = departingStation,
                ArrivingStation = arrivingStation,
                Route = route,
                CarNumber = carNumber,
                SeatNumber = seatNumber,
                Client = client,
                Date = date
            };
        }

        public void PrintRoutes()
        {
            Console.WriteLine("Список доступных маршрутов\n");
            TableService.Show(_routeService.GetAll(), new string[] { "Маршрут ", "Название"},
            s => s.Id,
            s => s.RouteName);
        }

        public void PrintStationsInRoute(int idRoute)
        {
            var route = _routeService.GetById(idRoute);
            Console.Clear();
            Console.WriteLine($"Список станций в маршруте {idRoute} {route.RouteName}\n");
            TableService.Show(route.Stations, new string[] { "Станция" },
            s => s.StationName);
        }

        private void PrintStations()
        {
            Console.WriteLine($"Список станций\n");
            TableService.Show(_stationService.GetAll(), new string[] { "Название" },
            s => s.StationName);
        }

        private bool CheckStation(string name) => _stationService.GetAll().Any(s => s.StationName.ToLower() == name.ToLower());
        
        private List<DateTime> GetDates()
        {
            DateTime start = DateTime.Now.Date;
            DateTime end = start.AddDays(10); //заменить потом на 60*

            return Enumerable.Range(0, (end - start).Days + 1)
                         .Select(offset => start.AddDays(offset))
                         .ToList();
        }

        private void PrintTickets(List<TicketDTO> tickets)
        {
            TableService.Show(tickets, new string[] { "Маршрут №", "Название марш.", "Вагон", "Место", "Станция отпр.", "Станция приб.", "Дата" },
                    t => t.Route.Id,
                    t => t.Route.RouteName,
                    t => t.CarNumber,
                    t => t.SeatNumber,
                    t => t.DepartingStation.StationName,
                    t => t.ArrivingStation.StationName,
                    t => t.Date);
        } 

        private void Draw(List<int> occupiedSeats, int totalSeats)
        {
            const int seatsPerRow = 4;

            int rows = (int)Math.Ceiling(totalSeats / (double)seatsPerRow);

            Console.WriteLine($"+{new string('-', seatsPerRow * 5)}+");

            for (int row = 0; row < rows; row++)
            {
                Console.Write("|");

                for (int seatInRow = 0; seatInRow < seatsPerRow; seatInRow++)
                {
                    int seatNumber = row * seatsPerRow + seatInRow + 1;

                    if (seatNumber > totalSeats) 
                    {
                        Console.Write("      ");
                    }
                    else if (occupiedSeats.Contains(seatNumber))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write($" {seatNumber,2} ");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write($" {seatNumber,2} ");
                        Console.ResetColor();
                    }

                    Console.Write(" ");
                }

                Console.WriteLine("|");
            }

            
            Console.WriteLine($"+{new string('-', seatsPerRow * 5)}+");
        }
    }
}
