using Data.MongoCollections;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModels
{
    public class SurveySessionViewModel
    {
        public Guid Id { get; set; }
        public Guid QuestionTemplateId { get; set; }
        public string UserId { get; set; }
        public DateTime DateCreated { get; set; }
        public string Result { get; set; }
        public List<SurveySessionResult> SurveySessionResults { get; set; }
    }

    public class SurveySessionAddModel
    {
        public Guid QuestionTemplateId { get; set; }
        public string UserId { get; set; }
        public string Result { get; set; }
        public List<SurveySessionResultAddModel> SurveySessionResults { get; set; }
    }

    public class SurveySessionResultAddModel
    {
        public Guid QuestionId { get; set; }
        public Guid AnswerId { get; set; }
    }

    public class UserTestResult
    {
        public double UserScore { get; set; }
        //public double TotalScore { get; set; }
        public SurveyResultViewModel SurveyResult { get; set; }
    }
}
