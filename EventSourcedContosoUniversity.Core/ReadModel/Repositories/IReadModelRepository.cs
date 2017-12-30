using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace EventSourcedContosoUniversity.Core.ReadModel.Repositories
{
    public interface IReadModelRepository
    {
        Task Add<T>(T item) where T : IMongoDocument, new();
        Task Update<T>(T item) where T : IMongoDocument, new();
        Task Delete<T>(T item) where T : IMongoDocument, new();
        Task<T> GetById<T>(Guid id) where T : IMongoDocument, new();
        Task<List<T>> All<T>() where T : IMongoDocument, new();
        IQueryable<T> AllAsQueryable<T>() where T : IMongoDocument, new();
        IMongoCollection<T> GetCollection<T>() where T : IMongoDocument, new();
    }
}
