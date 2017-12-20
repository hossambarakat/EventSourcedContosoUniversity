using System;

namespace EventSourcedContosoUniversity.Core.ReadModel.Repositories
{
    public class CollectionNameAttribute : Attribute
    {

        public CollectionNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
