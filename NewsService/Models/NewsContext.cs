using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace NewsService.Models
{
    public class NewsContext
    {
        //declare variables to connect to MongoDB database
        readonly MongoClient mongoClient;

        readonly IMongoDatabase mongoDb;

        public NewsContext(IConfiguration configuration)
        {
            //Initialize MongoClient and Database using connection string and database name from configuration
            mongoClient = new MongoClient(configuration.GetSection("MongoDB").GetSection("ConnectionString").Value);
            mongoDb = mongoClient.GetDatabase(configuration.GetSection("MongoDB").GetSection("NewsDatabase").Value);
        }
        //Define a MongoCollection to represent the News collection of MongoDB based on UserNews type
        public IMongoCollection<UserNews> News => mongoDb.GetCollection<UserNews>("userNews");
    }
}
