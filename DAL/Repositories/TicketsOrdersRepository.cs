using DAL.Entities;
using DAL.Interfaces;
using Npgsql;

namespace DAL.Repositories
{
    internal class TicketsOrdersRepository : IRepository<TicketOrdersEntity>
    {
        private readonly string _connectionString;
        public TicketsOrdersRepository(IConnectionString connection)
        {
            _connectionString = connection.ConnectionString;
        }

        public void Create(TicketOrdersEntity entity)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            var command = new NpgsqlCommand(@"INSERT INTO TicketsOrders (ticketid, orderid) VALUES (@TicketId, @OrderId)", connection);

            command.Parameters.AddWithValue("@TicketId", entity.TicketId);
            command.Parameters.AddWithValue("@OrderId", entity.OrderId);

            command.ExecuteNonQuery();
        }

        public void Delete(TicketOrdersEntity entity)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            var command = new NpgsqlCommand(@"DELETE FROM TicketOrders WHERE ticketid = @TicketId AND orderid = @OrderId", connection);

            command.Parameters.AddWithValue("@TicketId", entity.TicketId);
            command.Parameters.AddWithValue("@OrderId", entity.OrderId);

            command.ExecuteNonQuery();
        }

        public List<TicketOrdersEntity> GetAll()
        {
            var ticketOrders = new List<TicketOrdersEntity>();

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            var command = new NpgsqlCommand("SELECT ticketid, orderid FROM TicketOrders", connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                ticketOrders.Add(new TicketOrdersEntity
                {
                    TicketId = reader.GetInt32(0),
                    OrderId = reader.GetInt32(1)
                });
            }

            return ticketOrders;
        }

        public List<TicketOrdersEntity> GetByCriteria(Predicate<TicketOrdersEntity> predicate)
        {
            var allOrders = GetAll();
            return allOrders.FindAll(predicate);
        }

        public TicketOrdersEntity GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(TicketOrdersEntity entity)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            var command = new NpgsqlCommand(@"UPDATE TicketOrders
                                           SET orderid = @OrderId
                                           WHERE ticketid = @TicketId", connection);

            command.Parameters.AddWithValue("@TicketId", entity.TicketId);
            command.Parameters.AddWithValue("@OrderId", entity.OrderId);

            command.ExecuteNonQuery();
        }
    }
}
