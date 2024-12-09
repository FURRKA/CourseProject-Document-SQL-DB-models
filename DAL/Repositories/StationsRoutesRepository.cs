using DAL.Entities;
using DAL.Interfaces;
using Npgsql;

namespace DAL.Repositories
{
    internal class StationsRoutesRepository : IRepository<StationRoutesEntity>
    {
        private string _connection;

        public StationsRoutesRepository(IConnectionString connection) => _connection = connection.ConnectionString;

        public void Create(StationRoutesEntity entity)
        {
            var query = "INSERT INTO stationroutes (routeid, stationid) VALUES (@RouteId, @StationId) RETURNING id;";

            using (var connection = new NpgsqlConnection(_connection))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RouteId", entity.RouteId);
                    command.Parameters.AddWithValue("@StationId", entity.StationId);
                }
            }
        }

        public void Delete(StationRoutesEntity entity)
        {
            var query = "DELETE FROM stationroutes WHERE routeid = @RouteId AND stationid = @StationId;";

            using (var connection = new NpgsqlConnection(_connection))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RouteId", entity.RouteId);
                    command.Parameters.AddWithValue("@StationId", entity.StationId);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<StationRoutesEntity> GetAll()
        {
            var routes = new List<StationRoutesEntity>();
            var query = "SELECT routeid, stationid FROM stationroutes;";

            using (var connection = new NpgsqlConnection(_connection))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var stationRoute = new StationRoutesEntity
                            {
                                RouteId = reader.GetInt32(0),
                                StationId = reader.GetInt32(1)
                            };
                            routes.Add(stationRoute);
                        }
                    }
                }
            }

            return routes;
        }

        public List<StationRoutesEntity> GetByCriteria(Predicate<StationRoutesEntity> predicate)
        {
            var routes = GetAll();
            return routes.FindAll(predicate);
        }

        public StationRoutesEntity GetById(int id)
        {
            StationRoutesEntity stationRoute = null;
            var query = "SELECT id, routeid, stationid FROM stationroutes WHERE id = @Id;";

            using (var connection = new NpgsqlConnection(_connection))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            stationRoute = new StationRoutesEntity
                            {
                                Id = reader.GetInt32(0),
                                RouteId = reader.GetInt32(1),
                                StationId = reader.GetInt32(2)
                            };
                        }
                    }
                }
            }

            return stationRoute;
        }

        public void Update(StationRoutesEntity entity)
        {
            var query = "UPDATE stationroutes SET routeid = @RouteId, stationid = @StationId WHERE id = @Id;";

            using (var connection = new NpgsqlConnection(_connection))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RouteId", entity.RouteId);
                    command.Parameters.AddWithValue("@StationId", entity.StationId);
                    command.Parameters.AddWithValue("@Id", entity.Id);

                    command.ExecuteNonQuery();
                }
            }
        }
    }

}
