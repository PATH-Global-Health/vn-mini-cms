using Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.MongoCollections
{
    public class Part : BaseMongoCollection
    {
        public int Order { get; set; }
        public PartType Type { get; set; }
        public string Content { get; set; }
    }
}
