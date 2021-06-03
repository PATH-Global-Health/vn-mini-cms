using System;
using System.Collections.Generic;
using System.Text;

namespace Data.MongoCollections
{
    public class Post : BaseMongoCollection
    {
        public string Name { get; set; }
        public DateTime PublishDate { get; set; }
        public string Writter { get; set; }
        //public string UserId { get; set; }
        public List<Part> Parts { get; set; } = new List<Part>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Tag> Tags { get; set; } = new List<Tag>();
    }
}
