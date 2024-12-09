using DAL.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Entities
{
    public class Distances : IEntity
    {
        [BsonId]
        public int Id { get; set; }
        public int Stations1Id { get; set; }
        public int Stations2Id { get; set; }
        public int Value { get; set; }
    }
}
