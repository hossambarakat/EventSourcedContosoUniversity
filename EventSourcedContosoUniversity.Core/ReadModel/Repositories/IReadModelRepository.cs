using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventSourcedContosoUniversity.Core.ReadModel.Repositories
{
    public interface IReadModelRepository
    {
        Task Add<T>(T item) where T : IMongoDocument, new();
        Task Update<T>(T item) where T : IMongoDocument, new();
        Task Delete<T>(T item) where T : IMongoDocument, new();
        Task<T> GetById<T>(Guid id) where T : IMongoDocument, new();
        Task<List<T>> All<T>() where T : IMongoDocument, new();
    }
}
