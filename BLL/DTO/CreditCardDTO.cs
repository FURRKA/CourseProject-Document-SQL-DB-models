using BLL.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace BLL.DTO
{
    public class CreditCardDTO : IDTO
    {
        [BsonId]
        public int Id { get; set; } = 0;
        public string CardNumber { get; set; }
        public int CVC { get; set; }
        public double Value { get; set; }
    }
}
