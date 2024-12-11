using DAL.Entities;
using DAL.Interfaces;
using Npgsql;

namespace DAL.Repositories
{
    internal class StationsRepository : IRepository<StationsEntity>
    {
        private readonly string _connectionString;

        public StationsRepository(IConnectionString connection)
        {
            _connectionString = connection.ConnectionString;
        }

        public void Create(StationsEntity entity)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("INSERT INTO Stations (id, name) VALUES (@id, @name)", connection))
                {
                    command.Parameters.AddWithValue("@id", entity.Id);
                    command.Parameters.AddWithValue("@name", entity.StationName);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(StationsEntity entity)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("DELETE FROM Stations WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", entity.Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<StationsEntity> GetAll()
        {
            var stations = new List<StationsEntity>();
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT id, name FROM Stations", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        stations.Add(new StationsEntity
                        {
                            Id = reader.GetInt32(0),
                            StationName = reader.GetString(1)
                        });
                    }
                }
            }

            return stations;
        }

        public List<StationsEntity> GetByCriteria(Predicate<StationsEntity> predicate)
        {
            var allStations = GetAll();
            return allStations.FindAll(predicate);
        }

        public StationsEntity GetById(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT id, name FROM Stations WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new StationsEntity
                            {
                                Id = reader.GetInt32(0),
                                StationName = reader.GetString(1)
                            };
                        }
                    }
                }
            }

            return null;
        }

        public void Update(StationsEntity entity)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("UPDATE Stations SET name = @name WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@name", entity.StationName);
                    command.Parameters.AddWithValue("@id", entity.Id);

                    command.ExecuteNonQuery();
                }
            }
        }
        public int GetMaxNewId()
        {
            return GetAll().Max(c => c.Id) + 1;
        }

    }
}
