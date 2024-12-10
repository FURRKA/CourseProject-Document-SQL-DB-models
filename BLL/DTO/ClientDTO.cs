using BLL.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace BLL.DTO
{
    public class ClientDTO : IDTO
    {
        [BsonId]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public ClientDTO() { }
        public ClientDTO(string name, string lastname, string surname, string login, string password)
        {
            Name = name;
            LastName = lastname;
            Surname = surname;
            Login = login;
            Password = password;
        }
    }
}
