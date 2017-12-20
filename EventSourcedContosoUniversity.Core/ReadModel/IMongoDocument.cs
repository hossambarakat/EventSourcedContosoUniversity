using System;

namespace EventSourcedContosoUniversity.Core.ReadModel
{
    public interface IMongoDocument
    {
        Guid Id { get; }
    }
}
