using DAL.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

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
        [BsonDateTimeOptions(DateOnly = true)]
        public DateTime Date { get; set; }

    }
}
