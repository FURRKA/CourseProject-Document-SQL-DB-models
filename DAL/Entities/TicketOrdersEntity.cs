using DAL.Interfaces;

namespace DAL.Entities
{
    public class TicketOrdersEntity : IEntity
    {
        public int Id { get; set; } = 0;
        public int TicketId { get; set; }
        public int OrderId { get; set; }
    }
}
