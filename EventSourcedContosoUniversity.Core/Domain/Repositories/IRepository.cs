using System;
using System.Threading.Tasks;

namespace EventSourcedContosoUniversity.Core.Domain.Repositories
{
    public interface IRepository<T> where T : AggregateRoot
    {
        Task Save(T aggregate);
        Task<T> GetById(Guid id);
        //Task DeleteById(Guid id);
    }
}
