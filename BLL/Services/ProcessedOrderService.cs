using BLL.DTO;
using BLL.Interfaces;
using MongoDB.Driver;

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

        public void PrintActiveOrders(ClientDTO client)
        {
            Console.Clear();
            var activeOrders = _orderService.GetByCriteria(o => o.Tickets.Any(t => t.Client.Id == client.Id) &&
                   o.Date >= DateTime.Now.Date.Add(TimeSpan.FromDays(-1)) &&
                   o.Date <= DateTime.Now.Date.Add(TimeSpan.FromDays(10)));

            if (activeOrders.Count() > 0)
            {
                Console.WriteLine("Активные заказы:");
                PrintOrders(activeOrders);
                Console.WriteLine("Активные билеты:");
                activeOrders.ForEach(o => PrintTickets(o.Tickets));
            }
            else
                Console.WriteLine("У вас нету активных заказов");

        }
        public void CreateOrder(ClientDTO client)
        {
            var tickets = new List<TicketDTO>();
            ConsoleKey key;

            do
            {
                Console.Clear();
                Console.WriteLine("Регистрация билета");
                TicketDTO ticket = CreateTicket(client, tickets);
                if (ticket == null)
                    return;
                ticket.Id = tickets.Count == 0 ? _orderService.GetMaxNewId() : tickets.Max(t => t.Id) + 1;
                tickets.Add(ticket);
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

            Console.WriteLine("Нажмите на любую клавишу...");
            Console.ReadKey();
        }

        public void DeleteOrder(ClientDTO client)
        {
            Console.Clear();
            var activeOrders = _orderService.GetByCriteria(o => o.Tickets.Any(t => t.Client.Id == client.Id) && 
                   o.Date >= DateTime.Now.Date.Add(TimeSpan.FromDays(-1)) && 
                   o.Date <= DateTime.Now.Date.Add(TimeSpan.FromDays(10)));

            if (activeOrders.Count() > 0 )
            {
                Console.WriteLine("Активные заказы:");
                PrintOrders(activeOrders);
                Console.WriteLine("Активные билеты:");
                activeOrders.ForEach(o => PrintTickets(o.Tickets));

                Console.WriteLine("Вы хотите отменить билет или заказ?\n1 Заказ\n2 Билет");
                Console.Write("Ваш выбор: ");
                string choice = Console.ReadLine();
                var paidOrder = activeOrders.Where(o => o.Status == true).ToList();
                int id;
                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        if (paidOrder.Count > 0)
                        {
                            PrintOrders(paidOrder);
                            Console.WriteLine("Введите id заказа который хотите отменить");
                            if (Int32.TryParse(Console.ReadLine(), out id) && paidOrder.Any(o => o.Id == id))
                            {
                                var order = paidOrder.Find(o => o.Id == id);
                                if (order != null)
                                {
                                    if (_bankingService.AddTransaction(order.CreditsCard.CardNumber, order.CreditsCard.CVC, order.Cost))
                                    {
                                        order.Tickets.ForEach(_ticketService.Delete);
                                        _orderService.Delete(order);
                                        Console.WriteLine("Заказ успешно отменён");
                                    }
                                }
                            }
                            Console.ReadKey();

                        }
                        else
                        {
                            Console.WriteLine("У вас нету оплаченых заказов");
                            Console.ReadKey();
                        }
                        break;
                    case "2":
                        Console.Clear();
                        if (paidOrder.Count > 0)
                        {
                            paidOrder.ForEach(o => PrintTickets(o.Tickets));
                            Console.WriteLine("Введите id билета который хотите отменить");
                            if (Int32.TryParse(Console.ReadLine(), out id) && paidOrder.Any(o => o.Tickets.Select(t => t.Id).ToList().Contains(id)))
                            {
                                var order = paidOrder.Find(o => o.Tickets.Any(t => t.Id == id));
                                if (CancelTicket(order.Tickets, id, order.Id))
                                {
                                    Console.WriteLine("Билет отменён");
                                    Console.ReadKey();
                                }

                            }
                            else
                            {
                                Console.WriteLine("Введённый id не входит в данный список");
                                Console.ReadKey();
                            }

                        }
                        else
                        {
                            Console.WriteLine("У вас нету оплаченых заказов с билетами");
                            Console.ReadKey();
                        }
                        break;
                    default:
                        Console.WriteLine("Такого пункта нет в меню. Возвращение на главную страницу...");
                        break;
                }
            }
            else
                Console.WriteLine("У вас нету активных заказов");

        }

        public void PayOrders(ClientDTO client)
        {
            var unpaidOrders = _orderService.GetByCriteria(o => o.Tickets.Any(t => t.Client.Id == client.Id) &&
                   o.Status == false &&
                   o.Date >= DateTime.Now.Date.Add(TimeSpan.FromDays(-1)) &&
                   o.Date <= DateTime.Now.Date.Add(TimeSpan.FromDays(10)));

            if (unpaidOrders.Count() > 0)
            {
                Console.WriteLine("Неоплаченные заказы:");
                PrintOrders(unpaidOrders);

                double finalCost = unpaidOrders.Sum(o => o.Cost);
                Console.WriteLine($"К оплате {finalCost:f2}");
                do
                {
                    Console.Write("\nВведите номер карты: ");
                    string number = Console.ReadLine();
                    if (!_bankingService.CheckBankAccount(number))
                    {
                        Console.WriteLine("Карты с таким номером нету в системе банка");
                        Console.ReadKey();
                        continue;
                    }

                    Console.Write("\nВведите cvc карты: ");
                    if (!Int32.TryParse(Console.ReadLine(), out int cvc))
                    {
                        Console.WriteLine("Вы что-то ввели не так");
                        Console.ReadKey();
                        continue;
                    }

                    if (_bankingService.RemoveTransaction(number, cvc, finalCost))
                    {
                        foreach (var item in unpaidOrders)
                        {
                            item.Status = true;
                            item.CreditsCard = new CreditCardDTO { CardNumber = number, CVC = cvc };
                            _orderService.Update(item);
                        }

                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine("Оплата успешно завершена!");
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine("Оплата не прошла");
                    }

                    Console.ResetColor();
                    break;

                } while (true);
            }
            else
                Console.WriteLine("Неоплаченных заказов нет");

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
            TableService.Show(GetDates(), new string[] {"Доступные даты" }, d => d.Date.ToShortDateString());
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
            if (routes.Count == 0)
            {
                Console.WriteLine("Для данных станций нету маршрута");
                Console.ReadKey();
                return null;
            }
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

        public void PrintStations()
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
            TableService.Show(tickets, new string[] {"Id", "Маршрут №", "Название марш.", "Вагон", "Место", "Станция отпр.", "Станция приб.", "Дата" },
                    t => t.Id,
                    t => t.Route.Id,
                    t => t.Route.RouteName,
                    t => t.CarNumber,
                    t => t.SeatNumber,
                    t => t.DepartingStation.StationName,
                    t => t.ArrivingStation.StationName,
                    t => t.Date.ToShortDateString());
        } 

        private void PrintOrders(List<OrderDTO> orders)
        {
            TableService.Show(orders, new string[] { "Id", "Дата", "Статус", "Стоимость" },
                p => p.Id,
                p => p.Date.ToShortDateString(),
                p => p.Status ? "Оплачено" : "Неоплачено",
                p => Math.Round(p.Cost, 2)
            );
        }

        private bool CancelTicket(List<TicketDTO> tickets, int idTicket, int idOrder)
        {
            var ticket = tickets.Find(t => t.Id == idTicket);
            var order = _orderService.GetById(idOrder);
            if (order != null && ticket != null)
            {
                double cost = ticket.Route.GetLenth(ticket.DepartingStation.Id, ticket.ArrivingStation.Id) * 0.05;
                order.Tickets.Remove(ticket);
                order.Cost -= cost;
                _ticketService.Delete(ticket);
                _orderService.Update(order);
                if (_bankingService.AddTransaction(order.CreditsCard.CardNumber, order.CreditsCard.CVC, cost))
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.WriteLine("Транзакция отмены билета прошла успешно");
                    Console.ResetColor();
                    return true;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("Транзакция отмены не прошла");
                    Console.ResetColor();
                    return false;
                }
            }
            else
                return false;
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
