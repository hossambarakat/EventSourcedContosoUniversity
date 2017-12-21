using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourcedContosoUniversity.Core.ReadModel
{
    public class ReadModelSettings
    {
        public string MongoConnectionString { get; set; }
        public string MongoDatabase { get; set; }
    }
}
