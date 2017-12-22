using System;
using System.Threading.Tasks;
using EventSourcedContosoUniversity.Core.Extensions;
using EventStore.ClientAPI;
using MongoDB.Driver;

namespace EventSourcedContosoUniversity.Core.ReadModel.Repositories
{
    public class CatchupPositionRepository : MongoRepository, ICatchupPositionRepository
    {
        public CatchupPositionRepository(IMongoClient client, ReadModelSettings settings) : base(client, settings)
        {
        }
        public async Task SavePosition<T>(Position position) where T : IMongoDocument
        {
            var collectionName = Db.GetCollectionName<T>();
            ;
            var currentPosition = await Db.GetCollection<CatchupPosition>().Find(x => x.CollectionName == collectionName).SingleOrDefaultAsync();
            if(currentPosition==null)
            {
                await Db.GetCollection<CatchupPosition>().InsertOneAsync(new CatchupPosition
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
                await Db.GetCollection<CatchupPosition>().FindOneAndUpdateAsync(x => x.Id == currentPosition.Id, updateDefinition);
            }
           
        }
        public async Task<CatchupPosition> GetLastProcessedPosition<T>() where T : IMongoDocument
        {
            var collectionName = Db.GetCollectionName<T>();
            var currentPosition = await Db.GetCollection<CatchupPosition>().Find(x => x.CollectionName == collectionName).SingleOrDefaultAsync();
            return currentPosition;
        }
    }

    public interface ICatchupPositionRepository
    {
        Task<CatchupPosition> GetLastProcessedPosition<T>() where T : IMongoDocument;
        Task SavePosition<T>(Position position) where T : IMongoDocument;
    }
}
