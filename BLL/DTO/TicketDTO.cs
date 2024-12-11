using BLL.Interfaces;
using DAL.Entities;
using MongoDB.Bson.Serialization.Attributes;

namespace BLL.DTO
{
    public class TicketDTO : IDTO
    {
        [BsonId]
        public int Id { get; set; }
        public int CarNumber { get; set; }
        public int SeatNumber { get; set; }
        public StationDTO DepartingStation { get; set; }
        public StationDTO ArrivingStation { get; set; }

        [BsonDateTimeOptions(DateOnly =true)]
        public DateTime Date { get; set; }
        public RouteDTO Route { get; set; }
        public ClientDTO Client { get; set; }
    }
}
