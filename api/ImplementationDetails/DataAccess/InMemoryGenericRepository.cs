using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using Models;

namespace DataAccess
{
    public class InMemoryGenericRepository<T, R>
        : IGenericRepostitory<T, R>
    where R : BaseEntity<T>
    where T : struct
    {
        private readonly ConcurrentDictionary<T, R> _dictionary;

        private readonly List<Action<ConcurrentDictionary<T, R>>> _changes = new List<Action<ConcurrentDictionary<T, R>>>();

        public InMemoryGenericRepository(ConcurrentDictionary<T, R> dictionary)
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

        public Task<IEnumerable<R>> GetAll()
        {
            var allItems = _dictionary.Values.Select(x => x);

            return Task.FromResult(allItems);
        }

        public Task<R> GetById(T id)
        {
            var result = _dictionary[id];

            return Task.FromResult(result);
        }

        public Task<IEnumerable<R>> GetSome(Func<R, bool> condition)
        {
            var allItems = _dictionary.Values.Where(condition);

            return Task.FromResult(allItems);
        }

        public void Save(R entity)
        {
            _changes.Add(dict => SaveItem(entity, dict));
        }

        private void SaveItem(R item, ConcurrentDictionary<T, R> dictionary)
        {
            dictionary.TryAdd(item.Id, item);
        }

        private void DeleteItem(T id, ConcurrentDictionary<T, R> dictionary)
        {
            dictionary.TryRemove(id, out R item);
        }
    }
}

