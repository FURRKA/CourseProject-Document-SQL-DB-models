using DAL.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Entities
{
    public class OrderEntity : IEntity
    {
        [BsonId]
        public int Id { get; set; }
        public List<TicketEntity> Tickets { get; set; } = [];
        public CreditsCard CreditsCard { get; set; }
        public bool Status { get; set; } = false;
        public double Cost { get; set; }
        public DateTime Date { get; set; }
    }
}
