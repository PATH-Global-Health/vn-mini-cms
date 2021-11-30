using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.MongoCollections
{
    public class QuestionTemplate : BaseMongoCollection
    {
        public string Title { get; set; }
        public List<QuestionOrder> Questions { get; set; }
        public List<SurveyResult> SurveyResults { get; set; }
        public Guid? QuestionTemplateTypeId { get; set; }
    }

    public class SurveyResult : BaseMongoCollection
    {
        public double FromScore { get; set; }
        public double ToScore { get; set; }
    }

    public class QuestionTemplateType : BaseMongoCollection
    {

    }

    public class QuestionOrder : Question
    {
        public int Order { get; set; }
    }

}
