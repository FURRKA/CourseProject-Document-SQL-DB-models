using DAL.Interfaces;

namespace DAL.Entities
{
    public class RouteEntity : IEntity
    {
        public int Id { get; set; }
        public string RouteName { get; set; }
        public List<StationsEntity> Stations { get; set; } = [];
        public List<Distances> Distances { get; set; } = [];

        public int GetLenth(StationsEntity st1, StationsEntity st2)
        {
            int i1 = Stations.IndexOf(st1);
            int i2 = Stations.IndexOf(st2);
            
            return Distances
                .Skip(Math.Min(i1, i2))
                .Take(Math.Abs(i1 - i2))
                .Sum(d => d.Value);
        }
    }
}
