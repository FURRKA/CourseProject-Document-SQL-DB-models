using DAL.Interfaces;

namespace DAL.Entities
{
    public class TicketEntity : IEntity
    {
        public int Id { get; set; }
        public int CarNumber { get; set; }
        public int SeatNumber { get; set; }
        public StationsEntity DepartingStation { get; set; }
        public StationsEntity ArrivingStation { get; set; }
        public DateTime Date { get; set; }
        public RouteEntity Route { get; set; }
        public ClientsEntity Client { get; set; }

    }
}
