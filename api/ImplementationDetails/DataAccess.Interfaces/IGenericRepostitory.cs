using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace DataAccess.Interfaces
{
    public interface IGenericRepostitory<T, R>
        where R : BaseEntity<T>
        where T : struct
    {
        Task<R> GetById(T id);
        void Save(R entity);
        void Delete(T id);
        Task<IEnumerable<R>> GetAll();
        Task<IEnumerable<R>> GetSome(Func<R, bool> condition);
        Task Apply();
    }
}

