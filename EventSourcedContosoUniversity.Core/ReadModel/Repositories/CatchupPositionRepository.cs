using System;
using System.Threading.Tasks;
using EventSourcedContosoUniversity.Core.Extensions;
using EventStore.ClientAPI;
using MongoDB.Driver;

namespace EventSourcedContosoUniversity.Core.ReadModel.Repositories
{
    public class CatchupPositionRepository : MongoRepository
    {
        public CatchupPositionRepository(IMongoClient client) : base(client)
        {
        }
        public async Task SavePosition<T>(Position position) where T : IMongoDocument
        {
            var collectionName = _db.GetCollectionName<T>();
            ;
            var currentPosition = await _db.GetCollection<CatchupPosition>().Find(x => x.CollectionName == collectionName).SingleOrDefaultAsync();
            if(currentPosition==null)
            {
                await _db.GetCollection<CatchupPosition>().InsertOneAsync(new CatchupPosition
                {
                    Id= Guid.NewGuid(),
                    CollectionName = collectionName,
                    CommitPosition = position.CommitPosition,
                    PreparePosition = position.PreparePosition
                });
            }
            else
            {
                var updateDefinition = Builders<CatchupPosition>.Update.Set(x => x.CommitPosition, position.CommitPosition).Set(x=>x.PreparePosition , position.PreparePosition);
                await _db.GetCollection<CatchupPosition>().FindOneAndUpdateAsync(x => x.Id == currentPosition.Id, updateDefinition);
            }
           
        }
        public async Task<CatchupPosition> GetLastProcessedPosition<T>() where T : IMongoDocument
        {
            var collectionName = _db.GetCollectionName<T>();
            var currentPosition = await _db.GetCollection<CatchupPosition>().Find(x => x.CollectionName == collectionName).SingleOrDefaultAsync();
            return currentPosition;
        }
    }
}
