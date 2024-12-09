using DAL.Interfaces;

namespace DAL.Entities
{
    public class StationRoutesEntity : IEntity
    {
        public int Id { get; set; } = 0;
        public int StationId { get; set; }
        public int RouteId { get; set; }
    }
}
