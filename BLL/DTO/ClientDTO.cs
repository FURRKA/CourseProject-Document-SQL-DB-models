using BLL.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace BLL.DTO
{
    public class ClientDTO : IDTO
    {
        [BsonId]
        public int Id { get; set; }
        public string Name { get; set; } = "Тестовая запись";
        public string LastName { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
