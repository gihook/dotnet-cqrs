using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Services.Interfaces
{
    public interface IGenericService<T, R> where R : BaseEntity<T> where T : struct
    {
        Task<R> GetById(T id);
        Task<R> SaveAsync(R item);
        Task<R> DeleteAsync(R item);
        Task<IEnumerable<R>> GetAll();
        Task<IEnumerable<R>> GetSome(Func<R, bool> condition);
    }
}

