using System;
using System.Threading.Tasks;
using EventSourcedContosoUniversity.Core.Domain.Entities;

namespace EventSourcedContosoUniversity.Core.Domain.Repositories
{
    public interface IRepository<T> where T : AggregateRoot
    {
        Task Save(T aggregate);
        Task<T> GetById(Guid id);
        Task Delete(T aggregate);
    }
}
