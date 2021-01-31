using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using Services.Interfaces;
using Models;

namespace Services
{
    public class GenericService<T, R>
        : IGenericService<T, R> where R
        : BaseEntity<T> where T : struct
    {
        protected IGenericRepostitory<T, R> _repository;

        public GenericService(IGenericRepostitory<T, R> repository)
        {
            _repository = repository;
        }

        public async Task<R> DeleteAsync(R item)
        {
            _repository.Delete(item.Id);
            await _repository.Apply();

            return item;
        }

        public Task<IEnumerable<R>> GetAll()
        {
            return _repository.GetAll();
        }

        public Task<R> GetById(T id)
        {
            return _repository.GetById(id);
        }

        public Task<IEnumerable<R>> GetSome(Func<R, bool> condition)
        {
            return _repository.GetSome(condition);
        }

        public async Task<R> SaveAsync(R item)
        {
            _repository.Save(item);
            await _repository.Apply();

            return item;
        }
    }
}
