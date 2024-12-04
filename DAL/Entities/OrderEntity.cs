using DAL.Interfaces;

namespace DAL.Entities
{
    public class OrderEntity : IEntity
    {
        public int Id { get; set; }
        public List<TicketEntity> Tickets { get; set; } = [];
        public CreditsCard CreditsCard { get; set; }
        public bool Status { get; set; } = false;
        public double Cost { get; set; }

        public OrderEntity(double CostKM, params TicketEntity[] tickets) 
        {
            Tickets = tickets.ToList();
            Cost = Tickets.Sum(t => t.Route.GetLenth(t.DepartingStation, t.ArrivingStation)) * CostKM;
        }
    }
}
