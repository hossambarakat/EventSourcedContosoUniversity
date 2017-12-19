using System;

namespace EventSourcedContosoUniversity.Core
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
