using BLL.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace BLL.DTO
{
    public class DistancesDTO : IDTO
    {
        [BsonId]
        public int Id { get; set; }
        public int Stations1Id { get; set; }
        public int Stations2Id { get; set; }
        public int Value { get; set; }
    }
}
