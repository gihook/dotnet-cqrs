using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace DataAccess.Interfaces
{
    public interface IGenericRepostitory<T> where T : struct
    {
        Task<BaseEntity<T>> GetById(T id);
        void Save(BaseEntity<T> entity);
        void Delete(T id);
        Task<IEnumerable<BaseEntity<T>>> GetAll();
        Task<IEnumerable<BaseEntity<T>>> GetSome(Func<BaseEntity<T>, bool> condition);
        Task Apply();
    }
}

