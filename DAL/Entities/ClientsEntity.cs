using DAL.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Entities
{
    public class ClientsEntity : IEntity
    {
        [BsonId]
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Surname { get; set; }
        public string Role { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
