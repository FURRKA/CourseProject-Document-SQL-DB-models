using DAL.Entities;
using DAL.Interfaces;
using Npgsql;

namespace DAL.Repositories
{
    internal class ClientsRepository : IRepository<ClientsEntity>
    {
        private readonly string _connectionString;
        public ClientsRepository(IConnectionString connection)
        {
            _connectionString = connection.ConnectionString;
        }

        public void Create(ClientsEntity entity)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(
                    "INSERT INTO Clients (name, lastname, surname, role, login, password) VALUES (@name, @lastname, @surname, @role, @login, @password)", connection))
                {
                    command.Parameters.AddWithValue("@name", entity.Name);
                    command.Parameters.AddWithValue("@lastname", entity.LastName);
                    command.Parameters.AddWithValue("@surname", entity.Surname);
                    command.Parameters.AddWithValue("@role", entity.Role);
                    command.Parameters.AddWithValue("@login", entity.Login);
                    command.Parameters.AddWithValue("@password", entity.Password);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(ClientsEntity entity)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("DELETE FROM Clients WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", entity.Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<ClientsEntity> GetAll()
        {
            var clients = new List<ClientsEntity>();
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT id, name, lastname, surname, role, login, password FROM Clients", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        clients.Add(new ClientsEntity
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            LastName = reader.GetString(2),
                            Surname = reader.GetString(3),
                            Role = reader.GetString(4),
                            Login = reader.GetString(5),
                            Password = reader.GetString(6)
                        });
                    }
                }
            }

            return clients;
        }

        public List<ClientsEntity> GetByCriteria(Predicate<ClientsEntity> predicate)
        {
            var allClients = GetAll();
            return allClients.FindAll(predicate);
        }

        public ClientsEntity GetById(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT id, name, lastname, surname, role, login, password FROM Clients WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new ClientsEntity
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                LastName = reader.GetString(2),
                                Surname = reader.GetString(3),
                                Role = reader.GetString(4),
                                Login = reader.GetString(5),
                                Password = reader.GetString(6)
                            };
                        }
                    }
                }
            }

            return null;
        }

        public void Update(ClientsEntity entity)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(
                    "UPDATE Clients SET name = @name, lastname = @lastname, surname = @surname, role = @role, login = @login, password = @password WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@name", entity.Name);
                    command.Parameters.AddWithValue("@lastname", entity.LastName);
                    command.Parameters.AddWithValue("@surname", entity.Surname);
                    command.Parameters.AddWithValue("@role", entity.Role);
                    command.Parameters.AddWithValue("@login", entity.Login);
                    command.Parameters.AddWithValue("@password", entity.Password);
                    command.Parameters.AddWithValue("@id", entity.Id);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
