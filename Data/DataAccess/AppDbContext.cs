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
        //public IMongoCollection<Part> Parts => _db.GetCollection<Part>("parts");
        public IMongoCollection<Post> Posts => _db.GetCollection<Post>("posts");
        public IMongoCollection<MongoCollections.Tag> Tags => _db.GetCollection<MongoCollections.Tag>("tags");

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

            //if (!collectionNames.Any(name => name == "parts"))
            //{
            //    _db.CreateCollection("parts");
            //}
            if (!collectionNames.Any(name => name == "posts"))
            {
                _db.CreateCollection("posts");
            }
            if (!collectionNames.Any(name => name == "tags"))
            {
                _db.CreateCollection("tags");
            }
        }
    }
}
