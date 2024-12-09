using BLL.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace BLL.DTO
{
    public class StationDTO : IDTO
    {
        [BsonId]
        public int Id { get; set; }
        public string StationName { get; set; }
    }
}
