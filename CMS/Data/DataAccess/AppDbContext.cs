using Data.MongoCollections;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.DataAccess
{
    public class AppDbContext
    {
        private readonly IMongoDatabase _db;
        private IMongoClient _mongoClient;

        public AppDbContext(IMongoClient client, string databaseName)
        {
            _db = client.GetDatabase(databaseName);
            _mongoClient = client;
        }

        public IMongoCollection<Category> Categories => _db.GetCollection<Category>("categories");
        public IMongoCollection<Post> Posts => _db.GetCollection<Post>("posts");
        public IMongoCollection<MongoCollections.Tag> Tags => _db.GetCollection<MongoCollections.Tag>("tags");

        //Survey
        public IMongoCollection<QuestionTemplate> QuestionTemplates => _db.GetCollection<QuestionTemplate>("questionTemplates");
        public IMongoCollection<Question> Questions => _db.GetCollection<Question>("questions");
        public IMongoCollection<SurveySession> SurveySessions => _db.GetCollection<SurveySession>("surveySessions");
        public IMongoCollection<FileData> FileData => _db.GetCollection<FileData>("fileData");
        public IMongoCollection<QuestionTemplateType> QuestionTemplateTypes => _db.GetCollection<QuestionTemplateType>("questionTemplateTypes");

        public IClientSessionHandle StartSession()
        {
            return _mongoClient.StartSession();
        }

        public void CreateCollectionsIfNotExists()
        {
            var collectionNames = _db.ListCollectionNames().ToList();

            if (!collectionNames.Any(name => name == "categories"))
            {
                _db.CreateCollection("categories");
            }
            if (!collectionNames.Any(name => name == "posts"))
            {
                _db.CreateCollection("posts");
            }
            if (!collectionNames.Any(name => name == "tags"))
            {
                _db.CreateCollection("tags");
            }

            //Survey parts
            if (!collectionNames.Any(name => name == "questionTemplates"))
            {
                _db.CreateCollection("questionTemplates");
            }
            if (!collectionNames.Any(name => name == "questions"))
            {
                _db.CreateCollection("questions");
            }
            if (!collectionNames.Any(name => name == "surveySessions"))
            {
                _db.CreateCollection("surveySessions");
            }

            if (!collectionNames.Any(name => name == "fileData"))
            {
                _db.CreateCollection("fileData");
            }

            if (!collectionNames.Any(name => name == "questionTemplateTypes"))
            {
                _db.CreateCollection("questionTemplateTypes");
            }
        }
    }
}
