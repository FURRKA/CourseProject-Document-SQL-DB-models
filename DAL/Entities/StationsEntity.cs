using DAL.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Entities
{
    public class StationsEntity : IEntity
    {
        [BsonId]
        public int Id { get; set; }
        public string StationName { get; set; }
    }
}
