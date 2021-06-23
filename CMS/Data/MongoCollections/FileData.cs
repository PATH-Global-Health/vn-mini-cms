using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.MongoCollections
{
    public class FileData
    {
        [BsonId]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public byte[] Data { get; set; }
    }
}
