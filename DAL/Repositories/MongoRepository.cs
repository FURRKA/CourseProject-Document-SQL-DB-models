using DAL.Interfaces;
using MongoDB.Driver;

namespace DAL.Repositories
{
    internal class MongoRepository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly IMongoCollection<T> _collection;

        public MongoRepository(IMongoDatabase database, IConnectionString connection)
        {
            _collection = database.GetCollection<T>(connection.ConnectionString);
        }

        public void Create(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            entity.Id = _collection.Find(_ => true).ToList().Count > 0 ? _collection.Find(_ => true).ToList().Max(x => x.Id) + 1 : 0;
            _collection.InsertOne(entity);
        }

        public List<T> GetAll()
        {
            return _collection.Find(_ => true).ToList();
        }

        public T GetById(int id)
        {
            return _collection.Find(e => e.Id == id).FirstOrDefault();
        }

        public List<T> GetByCriteria(Predicate<T> predicate)
        {
            var allItems = GetAll();
            return allItems.Where(x => predicate(x)).ToList();
        }

        public void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _collection.ReplaceOne(e => e.Id == entity.Id, entity);
        }

        public void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _collection.DeleteOne(e => e.Id == entity.Id);
        }
    }
}
