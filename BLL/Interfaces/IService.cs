using DAL.Interfaces;

namespace BLL.Interfaces
{
    public interface IService<T> where T : class, IDTO
    {
        void Create(T item);
        List<T> GetAll();
        T GetById(int id);
        List<T> GetByCriteria(Predicate<T> predicate);
        void Update(T item);
        void Delete(T item);
    }
}
