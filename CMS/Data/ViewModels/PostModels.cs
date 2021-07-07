using Data.MongoCollections;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModels
{
    public class PostAddModel
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public DateTime PublishDate { get; set; }
        public string Writter { get; set; }
        public List<Guid> Categories { get; set; }
        public List<Guid> Tags { get; set; }
    }

    public class PostUpdateModel
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public DateTime PublishDate { get; set; }
        public string Writter { get; set; }
        public List<Guid> Categories { get; set; }
        public List<Guid> Tags { get; set; }
    }

    public class PostViewModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string IsDeleted { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime DateUpdated { get; set; } = DateTime.Now;
        public string Name { get; set; }
        public DateTime PublishDate { get; set; }
        public string Writter { get; set; }
        public List<Category> Categories { get; set; }
        public List<Tag> Tags { get; set; }
    }

}
