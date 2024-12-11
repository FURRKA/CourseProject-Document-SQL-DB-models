using DAL.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Entities
{
    public class TicketEntity : IEntity
    {
        [BsonId]
        public int Id { get; set; }
        public int CarNumber { get; set; }
        public int SeatNumber { get; set; }
        public StationsEntity DepartingStation { get; set; }
        public StationsEntity ArrivingStation { get; set; }

        [BsonDateTimeOptions(DateOnly = true)]
        public DateTime Date { get; set; }
        public RouteEntity Route { get; set; }
        public ClientsEntity Client { get; set; }

    }
}
