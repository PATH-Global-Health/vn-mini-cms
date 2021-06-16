using System;
using System.Collections.Generic;
using System.Text;

namespace Data.MongoCollections
{
    public class Question : BaseMongoCollection
    {
        public bool IsMultipleChoice { get; set; }
        public List<Answer> Answers { get; set; }
    }
    public class Answer : BaseMongoCollection
    {
        public double Score { get; set; }
    }
}
