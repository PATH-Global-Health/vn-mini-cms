using Data.MongoCollections;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModels
{
    public class QuestionAddModel
    {
        public string Description { get; set; }
        public bool IsMultipleChoice { get; set; }
        public List<AnswerAddModel> Answers { get; set; }
    }
    
    public class QuestionUpdateModel
    {
        public string Description { get; set; }
        public bool IsMultipleChoice { get; set; }
    }

    public class AnswerAddModel
    {
        public double Score { get; set; }
        public string Description { get; set; }
    }

    public class QuestionViewModel : BaseModel
    {
        public bool IsMultipleChoice { get; set; }
        public List<Answer> Answers { get; set; }
    }

    public class QuestionAddAnswerModel
    {
        public Guid QuestionId { get; set; }
        public List<AnswerAddModel> Answers { get; set; }
    }
}
