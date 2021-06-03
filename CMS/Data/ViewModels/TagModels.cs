using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModels
{
    public class TagAddModel
    {
        public string Description { get; set; }
    }

    public class TagUpdateModel : TagAddModel
    {
        public Guid Id { get; set; }
    }

    public class TagViewModel : TagUpdateModel
    {
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
