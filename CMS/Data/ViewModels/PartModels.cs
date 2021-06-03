using Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModels
{
    public class PartViewModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime DateUpdated { get; set; } = DateTime.Now;
        public int Order { get; set; }
        public PartType Type { get; set; }
        public string Content { get; set; }
    }

    public class PartAddModel
    {
        public int Order { get; set; }
        public PartType Type { get; set; }
        public string Content { get; set; }
    }

    public class PartUpdateModel
    {
        public int Order { get; set; }
        public PartType Type { get; set; }
        public string Content { get; set; }
    }

    public class AddPartToPostModel
    {
        public Guid PostId { get; set; }
        public List<PartAddModel> Parts { get; set; }
    }
}
