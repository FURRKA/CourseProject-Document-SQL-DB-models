namespace BLL.Interfaces
{
    public interface IService<T> where T : class, IDTO
    {
        void Create(T entity);
        List<T> GetAll();
        T GetById(int id);
        List<T> GetByCriteria(Predicate<T> predicate);
        void Update(T entity);
        void Delete(T entity);
    }
}
