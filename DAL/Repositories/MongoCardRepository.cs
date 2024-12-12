using DAL.Entities;
using DAL.Interfaces;
using MongoDB.Driver;

namespace DAL.Repositories
{
    internal class MongoCardRepository : MongoRepository<CreditsCard>, IFindByCardNumber
    {
        public MongoCardRepository(IMongoDatabase database, string collectionName) : base(database, collectionName)
        {
        }

        public CreditsCard FindByNumber(string number)
        {
            return GetAll().Find(c => number.Contains(c.CardNumber));
        }
    }
}
