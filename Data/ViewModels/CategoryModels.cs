using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModels
{
    public class CategoryAddModel
    {
        public string Description { get; set; }
    }

    public class CategoryUpdateModel : CategoryAddModel
    {
        public Guid Id { get; set; }
    }

    public class CategoryViewModel : CategoryUpdateModel
    {
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; } 
    }
}
