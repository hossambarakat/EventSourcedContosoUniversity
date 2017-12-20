using System;
using EventSourcedContosoUniversity.Core.ReadModel.Repositories;

namespace EventSourcedContosoUniversity.Core.ReadModel
{
    [CollectionName("CatchupPositions")]
    public class CatchupPosition : IMongoDocument
    {
        public Guid Id { get; set; }
        public string CollectionName { get; set; }
        public long CommitPosition { get; set; }
        public long PreparePosition { get; set; }
    }
}
