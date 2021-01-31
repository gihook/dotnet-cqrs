using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using Models;

namespace DataAccess
{
    public class InMemoryGenericRepository<T>
        : IGenericRepostitory<T> where T : struct
    {
        private readonly ConcurrentDictionary<T, BaseEntity<T>> _dictionary;

        private readonly List<Action<ConcurrentDictionary<T, BaseEntity<T>>>> _changes = new List<Action<ConcurrentDictionary<T, BaseEntity<T>>>>();

        public InMemoryGenericRepository(ConcurrentDictionary<T, BaseEntity<T>> dictionary)
        {
            _dictionary = dictionary;
        }

        public Task Apply()
        {
            foreach (var change in _changes)
            {
                change(_dictionary);
            }

            return Task.CompletedTask;
        }

        public void Delete(T id)
        {
            _changes.Add(dict => DeleteItem(id, dict));
        }

        public Task<IEnumerable<BaseEntity<T>>> GetAll()
        {
            var allItems = _dictionary.Values.Select(x => x);

            return Task.FromResult(allItems);
        }

        public Task<BaseEntity<T>> GetById(T id)
        {
            var result = _dictionary[id];

            return Task.FromResult(result);
        }

        public Task<IEnumerable<BaseEntity<T>>> GetSome(Func<BaseEntity<T>, bool> condition)
        {
            var allItems = _dictionary.Values.Where(condition);

            return Task.FromResult(allItems);
        }

        public void Save(BaseEntity<T> entity)
        {
            _changes.Add(dict => SaveItem(entity, dict));
        }

        private void SaveItem(BaseEntity<T> item, ConcurrentDictionary<T, BaseEntity<T>> dictionary)
        {
            dictionary.TryAdd(item.Id, item);
        }

        private void DeleteItem(T id, ConcurrentDictionary<T, BaseEntity<T>> dictionary)
        {
            dictionary.TryRemove(id, out BaseEntity<T> item);
        }
    }
}

