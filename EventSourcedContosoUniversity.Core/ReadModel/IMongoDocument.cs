using System;

namespace EventSourcedContosoUniversity.Core
{
    public interface IMongoDocument
    {
        Guid Id { get; }
    }
}
