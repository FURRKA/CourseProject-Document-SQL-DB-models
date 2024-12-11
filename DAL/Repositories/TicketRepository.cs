using DAL.Entities;
using DAL.Interfaces;
using Npgsql;

namespace DAL.Repositories
{
    internal class TicketRepository : IRepository<TicketEntity>
    {
        private readonly string _connection;
        private readonly IRepository<ClientsEntity> clientsRepository;
        private readonly IRepository<StationsEntity> stationsRepository;
        private readonly IRepository<RouteEntity> routRepository;

        public TicketRepository(
            IRepository<ClientsEntity> clientsRepository,
            IRepository<StationsEntity> stationsRepository,
            IRepository<RouteEntity> routRepository,
            IConnectionString connection)
        {
            this.clientsRepository = clientsRepository;
            this.stationsRepository = stationsRepository;
            this.routRepository = routRepository;
            _connection = connection.ConnectionString;
        }
        public void Create(TicketEntity entity)
        {
            using var connection = new NpgsqlConnection(_connection);
            connection.Open();

            var command = new NpgsqlCommand(@"INSERT INTO Tickets (clientid, routeid, seatnumber, carnumber, departingstation, arrivingstation, date) VALUES (@ClientId, @RouteId, @SeatNumber, @CarNumber, @DepartingStation, @ArrivingStation, @Date)", connection);

            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@ClientId", entity.Client.Id);
            command.Parameters.AddWithValue("@RouteId", entity.Route.Id);
            command.Parameters.AddWithValue("@SeatNumber", entity.SeatNumber);
            command.Parameters.AddWithValue("@CarNumber", entity.CarNumber);
            command.Parameters.AddWithValue("@DepartingStation", entity.DepartingStation.StationName);
            command.Parameters.AddWithValue("@ArrivingStation", entity.ArrivingStation.StationName);
            command.Parameters.AddWithValue("@Date", entity.Date);

            command.ExecuteNonQuery();
        }

        public void Delete(TicketEntity entity)
        {
            using var connection = new NpgsqlConnection(_connection);
            connection.Open();
            var command = new NpgsqlCommand(@"DELETE FROM Tickets WHERE id = @Id", connection);
            command.Parameters.AddWithValue("@Id", entity.Id);

            command.ExecuteNonQuery();
        }

        public List<TicketEntity> GetAll()
        {
            var tickets = new List<TicketEntity>();

            using (var connection = new NpgsqlConnection(_connection))
            {
                connection.Open();
                var command = new NpgsqlCommand(@"SELECT * FROM Tickets", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var client = clientsRepository.GetById(reader.GetInt32(1));
                        var departingStation = stationsRepository.GetByCriteria(s => s.StationName.ToLower().Contains(reader.GetString(5).ToLower()))[0];
                        var arrivingStation = stationsRepository.GetByCriteria(s => s.StationName.ToLower().Contains(reader.GetString(6).ToLower()))[0];
                        var route = routRepository.GetById(reader.GetInt32(2));

                        tickets.Add(new TicketEntity
                        {
                            Id = reader.GetInt32(0),
                            Client = client,
                            DepartingStation = departingStation,
                            ArrivingStation = arrivingStation,
                            Route = route,
                            SeatNumber = reader.GetInt32(3),
                            CarNumber = reader.GetInt32(4),
                            Date = reader.GetDateTime(7)
                        });
                    }
                }

                return tickets;
            }
        }

        public List<TicketEntity> GetByCriteria(Predicate<TicketEntity> predicate)
        {
            return GetAll().FindAll(predicate);
        }

        public TicketEntity GetById(int id)
        {
            return GetAll().Find(t => t.Id == id);
        }

        public void Update(TicketEntity entity)
        {
            using var connection = new NpgsqlConnection(_connection);
            connection.Open();

            var command = new NpgsqlCommand(@"UPDATE Tickets
                SET clientid = @ClientId,
                    routeid = @RouteId,
                    seatnumber = @SeatNumber,
                    carnumber = @CarNumber,
                    departingStation = @DepartingStation,
                    arrivingStation = @ArrivingStation,
                    date = @Date
                WHERE id = @Id;", connection);

            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@ClientId", entity.Client.Id);
            command.Parameters.AddWithValue("@RouteId", entity.Route.Id);
            command.Parameters.AddWithValue("@SeatNumber", entity.SeatNumber);
            command.Parameters.AddWithValue("@CarNumber", entity.CarNumber);
            command.Parameters.AddWithValue("@DepartingStation", entity.DepartingStation.StationName);
            command.Parameters.AddWithValue("@ArrivingStation", entity.ArrivingStation.StationName);
            command.Parameters.AddWithValue("@Date", entity.Date.ToString());

            command.ExecuteNonQuery();
        }
        public int GetMaxNewId()
        {
            return GetAll().Max(c => c.Id) + 1;
        }
    }
}
