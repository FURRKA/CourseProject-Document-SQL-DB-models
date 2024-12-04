namespace DAL.Interfaces
{
    public interface IRepository<T> where T: class, IEntity
    {
        void Create(T entity);
        List<T> GetAll();
        T GetById(int id);
        List<T> GetByCriteria(Predicate<T> predicate);
        void Update(T entity);
        void Delete(T entity);
    }
}
