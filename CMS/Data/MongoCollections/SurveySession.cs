using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.MongoCollections
{
    public class SurveySession
    {
        [BsonId]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid QuestionTemplateId { get; set; }
        public string UserId { get; set; }
        public DateTime DateCreated { get; set; }
        public string Result { get; set; }
        public List<SurveySessionResult> SurveySessionResults { get; set; }
    }

    public class SurveySessionResult
    {
        public Question Question { get; set; }
        public Answer Answer { get; set; }
    }
}
