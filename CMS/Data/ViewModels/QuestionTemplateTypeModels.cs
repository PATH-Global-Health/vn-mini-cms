using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModels
{
    public class QuestionTemplateTypeAddModel
    {
        public string Description { get; set; }
    }

    public class QuestionTemplateTypeUpdateModel : TagAddModel
    {
        public Guid Id { get; set; }
    }

    public class QuestionTemplateTypeViewModel : TagUpdateModel
    {
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
