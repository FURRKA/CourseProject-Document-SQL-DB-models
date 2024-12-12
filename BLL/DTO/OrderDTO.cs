using BLL.Interfaces;
using DAL.Entities;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

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
        [BsonDateTimeOptions(DateOnly = true)]
        public DateTime Date { get; set; }

        public void CalculateCost()
        {
            Cost = Tickets.Sum(t => t.Route.GetLenth(t.DepartingStation.Id, t.ArrivingStation.Id)) * 0.05;
        }
    }
}
