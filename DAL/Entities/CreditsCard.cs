using DAL.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Entities
{
    public class CreditsCard : IEntity
    {
        [BsonId]
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public int CVC { get; set; }
        public double Value { get; set; }
    }
}
