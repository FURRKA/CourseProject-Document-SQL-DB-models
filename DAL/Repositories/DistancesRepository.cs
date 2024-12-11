using DAL.Entities;
using DAL.Interfaces;
using Npgsql;

namespace DAL.Repositories
{
    internal class DistancesRepository : IRepository<Distances>
    {
        private readonly string _connectionString;

        public DistancesRepository(IConnectionString connection)
        {
            _connectionString = connection.ConnectionString;
        }

        public void Create(Distances entity)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var command = new NpgsqlCommand(
                "INSERT INTO Distances (Id, Stations1Id, Stations2Id, Value) VALUES (@Id, @Stations1Id, @Stations2Id, @Value)",
                connection);
            command.Parameters.AddWithValue("Id", entity.Id);
            command.Parameters.AddWithValue("Stations1Id", entity.Stations1Id);
            command.Parameters.AddWithValue("Stations2Id", entity.Stations2Id);
            command.Parameters.AddWithValue("Value", entity.Value);
            command.ExecuteNonQuery();
        }

        public List<Distances> GetAll()
        {
            var distances = new List<Distances>();
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var command = new NpgsqlCommand("SELECT * FROM Distances", connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                distances.Add(new Distances
                {
                    Id = reader.GetInt32(0),
                    Stations1Id = reader.GetInt32(1),
                    Stations2Id = reader.GetInt32(2),
                    Value = reader.GetInt32(3)
                });
            }

            return distances;
        }

        public Distances GetById(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var command = new NpgsqlCommand("SELECT * FROM Distances WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("Id", id);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Distances
                {
                    Id = reader.GetInt32(0),
                    Stations1Id = reader.GetInt32(1),
                    Stations2Id = reader.GetInt32(2),
                    Value = reader.GetInt32(3)
                };
            }

            throw new InvalidOperationException($"Distance with Id {id} not found.");
        }

        public List<Distances> GetByCriteria(Predicate<Distances> predicate)
        {
            var allDistances = GetAll();
            return allDistances.FindAll(predicate);
        }

        public void Update(Distances entity)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var command = new NpgsqlCommand(
                "UPDATE Distances SET Stations1Id = @Stations1Id, Stations2Id = @Stations2Id, Value = @Value WHERE Id = @Id",
                connection);
            command.Parameters.AddWithValue("Id", entity.Id);
            command.Parameters.AddWithValue("Stations1Id", entity.Stations1Id);
            command.Parameters.AddWithValue("Stations2Id", entity.Stations2Id);
            command.Parameters.AddWithValue("Value", entity.Value);
            if (command.ExecuteNonQuery() == 0)
                throw new InvalidOperationException($"Distance with Id {entity.Id} not found.");
        }

        public void Delete(Distances entity)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var command = new NpgsqlCommand("DELETE FROM Distances WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("Id", entity.Id);
            if (command.ExecuteNonQuery() == 0)
                throw new InvalidOperationException($"Distance with Id {entity.Id} not found.");
        }
        public int GetMaxNewId()
        {
            return GetAll().Max(c => c.Id) + 1;
        }
    }
}
