using AutoMapper;
using BLL.Interfaces;
using DAL.Interfaces;

namespace BLL.Services
{
    public abstract class Service<TEntity, TDTO> : IService<TDTO> 
        where TDTO : class, IDTO
        where TEntity : class, IEntity
    {
        protected readonly IRepository<TEntity> _repository;
        protected readonly IMapper _mapper;
        public Service(IRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public void Create(TDTO item)
        {
            var obj = _mapper.Map<TEntity>(item);
            _repository.Create(obj);
        }

        public List<TDTO> GetAll()
        {
            return _repository.GetAll()
                .Select(_mapper.Map<TDTO>)
                .ToList();
        }

        public TDTO GetById(int id)
        {
            var obj = _repository.GetById(id);
            return _mapper.Map<TDTO>(obj);
        }

        public List<TDTO> GetByCriteria(Predicate<TDTO> predicate)
        {
            return GetAll().FindAll(predicate);
        }

        public void Update(TDTO item)
        {
            var obj = _mapper.Map<TEntity>(item);
            _repository.Update(obj);
        }

        public void Delete(TDTO item)
        {
            var obj = _mapper.Map<TEntity>(item);
            _repository.Delete(obj);
        }

        public int GetMaxNewId() => _repository.GetMaxNewId();
    }
}
