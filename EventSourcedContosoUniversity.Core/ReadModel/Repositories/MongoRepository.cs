using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventSourcedContosoUniversity.Core.Extensions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace EventSourcedContosoUniversity.Core
{
    public class MongoRepository : IReadModelRepository
    {
        private IMongoClient _client;
        //TODO: read db name from config
        protected IMongoDatabase _db { get { return _client.GetDatabase("contosouni"); } }
        public MongoRepository(IMongoClient client)
        {
            _client = client;
        }
        
        public Task Delete<T>(T item) where T : IMongoDocument, new()
        {
            return _db.GetCollection<T>().DeleteOneAsync(x=> x.Id == item.Id);
        }
        
        public Task<T> GetById<T>(Guid id) where T : IMongoDocument, new()
        {
            return _db.GetCollection<T>().AsQueryable().Where(x=>x.Id==id).SingleOrDefaultAsync();
        }
        public Task<List<T>> All<T>() where T : IMongoDocument, new()
        {
            return _db.GetCollection<T>().AsQueryable().ToListAsync();
        }
        
        public Task Add<T>(T item) where T : IMongoDocument, new()
        {
            return _db.GetCollection<T>().InsertOneAsync(item);
        }
        
        public Task Update<T>(T item) where T : IMongoDocument, new()
        {
            return _db.GetCollection<T>().ReplaceOneAsync(x => x.Id == item.Id, item);
        }
    }
}
