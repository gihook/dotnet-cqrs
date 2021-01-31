using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using Models;

namespace DataAccess
{
    public class InMemoryGenericRepository<R>
        : IGenericRepostitory<int, R> where R : BaseEntity<int>
    {
        private readonly ConcurrentDictionary<int, R> _dictionary;

        private List<Action<ConcurrentDictionary<int, R>>> _changes;

        public InMemoryGenericRepository(ConcurrentDictionary<int, R> dictionary)
        {
            _dictionary = dictionary;
            ResetChanges();
        }

        public Task Apply()
        {
            foreach (var change in _changes)
            {
                change(_dictionary);
            }

            ResetChanges();

            return Task.CompletedTask;
        }

        private void ResetChanges()
        {
            _changes = new List<Action<ConcurrentDictionary<int, R>>>();
        }

        public void Delete(int id)
        {
            _changes.Add(dict => DeleteItem(id, dict));
        }

        public Task<IEnumerable<R>> GetAll()
        {
            var allItems = _dictionary.Values.Select(x => x);

            return Task.FromResult(allItems);
        }

        public Task<R> GetById(int id)
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

        public void Update(R entity)
        {
            _changes.Add(dict => UpdateItem(entity, dict));
        }

        private void UpdateItem(R item, ConcurrentDictionary<int, R> dictionary)
        {
            dictionary[item.Id] = item;
        }

        private void SaveItem(R item, ConcurrentDictionary<int, R> dictionary)
        {
            // NOTE: naive implementation; probably good enough
            var currentIndex = dictionary.Keys.Count();
            item.Id = currentIndex;
            dictionary.TryAdd(currentIndex, item);
        }

        private void DeleteItem(int id, ConcurrentDictionary<int, R> dictionary)
        {
            dictionary.TryRemove(id, out R item);
        }
    }
}

