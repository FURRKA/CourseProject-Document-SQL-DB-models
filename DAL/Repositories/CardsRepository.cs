using DAL.Entities;
using DAL.Interfaces;
using Npgsql;

namespace DAL.Repositories
{
    internal class CardsRepository : IRepository<CreditsCard>, IFindByCardNumber
    {
        private readonly string _connectionString;

        public CardsRepository(IConnectionString connection)
        {
            _connectionString = connection.ConnectionString;
        }

        public void Create(CreditsCard entity)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            var command = new NpgsqlCommand(@"INSERT INTO Cards (Id, Number, cvc, Value) 
                                           VALUES (@Id, @CardNumber, @Cvc, @Value)", connection);

            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@CardNumber", entity.CardNumber);
            command.Parameters.AddWithValue("@Cvc", entity.CVC);
            command.Parameters.AddWithValue("@Value", entity.Value);

            command.ExecuteNonQuery();
        }

        public void Delete(CreditsCard entity)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            var command = new NpgsqlCommand(@"DELETE FROM Cards 
                                           WHERE Id = @Id", connection);

            command.Parameters.AddWithValue("@Id", entity.Id);

            command.ExecuteNonQuery();
        }

        public CreditsCard FindByNumber(string number)
        {
            return GetAll().Find(card => card.CardNumber.ToLower().Contains(number.ToLower()));
        }

        public List<CreditsCard> GetAll()
        {
            var cards = new List<CreditsCard>();

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            var command = new NpgsqlCommand("SELECT Id, Number, CVC, Value FROM Cards", connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                cards.Add(new CreditsCard
                {
                    Id = reader.GetInt32(0),
                    CardNumber = reader.GetString(1),
                    CVC = reader.GetInt32(2),
                    Value = reader.GetDouble(3)
                });
            }

            return cards;
        }

        public List<CreditsCard> GetByCriteria(Predicate<CreditsCard> predicate)
        {
            var allCards = GetAll();
            return allCards.FindAll(predicate);
        }

        public CreditsCard GetById(int id)
        {
            return GetAll().Find(c => c.Id == id);
        }

        public void Update(CreditsCard entity)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            var command = new NpgsqlCommand(
                @"UPDATE Cards
                SET Number = @CardNumber,
                CVC = @CVC,
                Value = @Value
                WHERE Id = @Id", connection);

            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@CardNumber", entity.CardNumber);
            command.Parameters.AddWithValue("@CVC", entity.CVC);
            command.Parameters.AddWithValue("@Value", entity.Value);

            command.ExecuteNonQuery();
        }
    }
}
