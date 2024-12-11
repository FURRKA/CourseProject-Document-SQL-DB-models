using DAL.Entities;
using DAL.Interfaces;
using Npgsql;

namespace DAL.Repositories
{
    internal class OrderRepository : IRepository<OrderEntity>
    {
        private readonly string _connectionString;
        private readonly IRepository<TicketEntity> ticketRepository;
        private readonly IRepository<TicketOrdersEntity> ticketOrdersRepository;
        private readonly IRepository<CreditsCard> cardsRepository;

        //Убрать в конфиг
        private readonly double COSTKM = 0.5;
        public OrderRepository(
            IRepository<TicketEntity> ticketRepository,
            IRepository<TicketOrdersEntity> ticketOrdersRepository,
            IRepository<CreditsCard> cardsRepository,
            IConnectionString connection
            )
        {
            _connectionString = connection.ConnectionString;
            this.ticketRepository = ticketRepository;
            this.ticketOrdersRepository = ticketOrdersRepository;
            this.cardsRepository = cardsRepository;
        }

        public void Create(OrderEntity entity)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            var command = new NpgsqlCommand(@"INSERT INTO Orders (cardid, cost, paid, date) 
                    VALUES (@CardId, @Cost, @Paid, @Date)", connection);
            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@CardId", entity.CreditsCard.Id);
            command.Parameters.AddWithValue("@Cost", entity.Cost);
            command.Parameters.AddWithValue("@Paid", entity.Status);
            command.Parameters.AddWithValue("@Date", entity.Date);

            if (command.ExecuteNonQuery() > 0)
            {
                var ticketsOrder = entity.Tickets.Select(item => new TicketOrdersEntity
                {
                    OrderId = entity.Id,
                    TicketId = item.Id
                }).ToList();

                ticketsOrder.ForEach(ticketOrdersRepository.Create);
            }
        }

        public void Delete(OrderEntity entity)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            var command = new NpgsqlCommand(@"DELETE FROM Orders WHERE id = @Id", connection);
            command.Parameters.AddWithValue("@Id", entity.Id);

            if (command.ExecuteNonQuery() > 0)
            {
                var ticketsOrder = entity.Tickets.Select(t => new TicketOrdersEntity 
                { 
                    OrderId = entity.Id,
                    TicketId = t.Id 
                }).ToList();

                ticketsOrder.ForEach(ticketOrdersRepository.Delete);
                entity.Tickets.ForEach(ticketRepository.Delete);
            }
        }

        public List<OrderEntity> GetAll()
        {
            var orders = new List<OrderEntity>();

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            var command = new NpgsqlCommand(@"SELECT * FROM Orders", connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                int orderId = reader.GetInt32(0);
                var ticketsIds = ticketOrdersRepository.GetByCriteria(t => t.OrderId == orderId).Select(t => t.TicketId);
                var tickets = ticketRepository.GetByCriteria(t => ticketsIds.Contains(t.Id));
                var card = cardsRepository.GetById(reader.GetInt32(1));
                double cost = tickets.Sum(t => t.Route.GetLenth(t.DepartingStation.Id, t.ArrivingStation.Id) * COSTKM);

                orders.Add(new OrderEntity
                {
                    Id = orderId,
                    Tickets = tickets,
                    CreditsCard = card,
                    Cost = cost
                });
            }

            return orders;
        }

        public List<OrderEntity> GetByCriteria(Predicate<OrderEntity> predicate)
        {
            return GetAll().FindAll(predicate).ToList();
        }

        public OrderEntity GetById(int id)
        {
            return GetAll().Find(o => o.Id == id);
        }

        public void Update(OrderEntity entity)
        { 
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            var command = new NpgsqlCommand(@"UPDATE Orders SET 
                    cardid = @CardId,
                    cost = @Cost,
                    paid = @Paid,
                    date = @Date
                    WHERE id = @Id", connection);
            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@CardId", entity.CreditsCard.Id);
            command.Parameters.AddWithValue("@Cost", entity.Cost);
            command.Parameters.AddWithValue("@Paid", entity.Status);
            command.Parameters.AddWithValue("@Date", entity.Date);

            command.ExecuteNonQuery();
        }
        public int GetMaxNewId()
        {
            return GetAll().Max(c => c.Id) + 1;
        }
    }
}