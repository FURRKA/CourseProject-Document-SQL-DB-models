using DAL.Entities;
using DAL.Interfaces;
using Npgsql;

namespace DAL.Repositories
{
    internal class RoutesRepository : IRepository<RouteEntity>
    {
        private readonly string _connectionString;
        private readonly IRepository<StationsEntity> stationRepository;
        private readonly IRepository<Distances> distancesRepository;
        private readonly IRepository<StationRoutesEntity> stationRoutRepository;

        public RoutesRepository(
            IRepository<StationsEntity> stationRepository,
            IRepository<Distances> distancesRepository,
            IRepository<StationRoutesEntity> stationRoutRepository,
            IConnectionString connection)
        {
            _connectionString = connection.ConnectionString;
            this.stationRoutRepository = stationRoutRepository;
            this.stationRepository = stationRepository;
            this.distancesRepository = distancesRepository;
        }

        public void Create(RouteEntity entity)
        {
            throw new NotImplementedException();
        }
        
        public List<RouteEntity> GetAll()
        {
            var routes = new List<RouteEntity>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                var command = new NpgsqlCommand("Select * from Routes", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var stations = stationRepository
                                    .GetByCriteria(s => stationRoutRepository
                                    .GetByCriteria(r => r.RouteId == reader.GetInt32(0)).Select(r => r.StationId).Contains(s.Id));

                        var distances = distancesRepository.GetByCriteria(d => stations.Any(s => d.Stations1Id == s.Id) && stations.Any(s => d.Stations2Id == s.Id));
                        
                        routes.Add(
                            new RouteEntity
                            {
                                Id = reader.GetInt32(0),
                                RouteName = reader.GetString(1),
                                Stations = stations,
                                Distances = distances
                            }
                        );
                    }
                }
            }

            return routes;
        }

        public RouteEntity GetById(int id)
        {
            return GetAll().Find(x => x.Id == id);
        }

        public List<RouteEntity> GetByCriteria(Predicate<RouteEntity> predicate)
        {
            return GetAll().FindAll(predicate);
        }

        public void Update(RouteEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(RouteEntity entity)
        {
            throw new NotImplementedException();
        }
        public int GetMaxNewId()
        {
            return GetAll().Max(c => c.Id) + 1;
        }
    }
}
