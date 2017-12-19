using System;
using MongoDB.Driver;

namespace EventSourcedContosoUniversity.Core.Extensions
{
    public static class MongoDatabaseExtensions
    {
        public static IMongoCollection<TDocument> GetCollection<TDocument>(this IMongoDatabase mongoDatabase)
        {
            var collectionName = mongoDatabase.GetCollectionName<TDocument>();

            return mongoDatabase.GetCollection<TDocument>(collectionName);
        }
        public static string GetCollectionName<TDocument>(this IMongoDatabase mongoDatabase)
        {
            var collectionNameAttribute = Attribute.GetCustomAttribute(typeof(TDocument), typeof(CollectionNameAttribute)) as CollectionNameAttribute;
            var collectionName = typeof(TDocument).Name;
            if (collectionNameAttribute != null)
            {
                collectionName = collectionNameAttribute.Name;
            }
            return collectionName;
        }
    }
}
