using Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.MongoCollections
{
    public class QuestionTemplateAddModel
    {
        public Guid QuestionTemplateTypeId { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public List<Guid> Questions { get; set; }
        public List<SurveyResultAddModel> SurveyResults { get; set; }
    }

    public class SurveyResultAddModel
    {
        public string Description { get; set; }
        public double FromScore { get; set; }
        public double ToScore { get; set; }
    }

    public class SurveyResultViewModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public double FromScore { get; set; }
        public double ToScore { get; set; }
    }

    public class SurveyResultUpdateModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public double FromScore { get; set; }
        public double ToScore { get; set; }
    }

    public class QuestionTemplateSuveyResultAddModel
    {
        public Guid Id { get; set; }
        public List<SurveyResultAddModel> SurveyResults { get; set; }
    }

    public class QuestionTemplateSuveyResultDeleteModel
    {
        public Guid Id { get; set; }
        public List<Guid> SurveyResults { get; set; }
    }

    public class QuestionTemplateQuestionModel
    {
        public Guid Id { get; set; }
        public List<Guid> Questions { get; set; }
    }

    public class QuestionTemplateViewModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public List<QuestionViewModel> Questions { get; set; }
        public List<SurveyResultViewModel> SurveyResults { get; set; }
        public Guid? QuestionTemplateTypeId { get; set; }
    }

    public class QuestionTemplateUserModel: QuestionTemplateViewModel
    {
        public bool IsCompleted { get; set; }
    }

    public class QuestionTemplateUpdateModel
    {
        public string Description { get; set; }
        public string Title { get; set; }
        public List<Guid> Questions { get; set; }
        public Guid QuestionTemplateTypeId { get; set; }
    }
}
