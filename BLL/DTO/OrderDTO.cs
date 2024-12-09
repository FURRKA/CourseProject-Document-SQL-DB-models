using BLL.Interfaces;
using DAL.Entities;
using MongoDB.Bson.Serialization.Attributes;

namespace BLL.DTO
{
    public class OrderDTO : IDTO
    {
        [BsonId]
        public int Id { get; set; }
        public List<TicketDTO> Tickets { get; set; } = [];
        public CreditCardDTO CreditsCard { get; set; }
        public bool Status { get; set; } = false;
        public double Cost { get; set; }
        public DateTime Date { get; set; }
    }
}
