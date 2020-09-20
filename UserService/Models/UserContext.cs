using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
namespace UserService.Models
{
    public class UserContext
    {
        //declare variables to connect to MongoDB database
        readonly MongoClient mongoClient;

        readonly IMongoDatabase mongoDb;

        public UserContext(IConfiguration configuration)
        {
            //Initialize MongoClient and Database using connection string and database name from configuration
            mongoClient = new MongoClient(configuration.GetSection("MongoDB").GetSection("ConnectionString").Value);
            mongoDb = mongoClient.GetDatabase(configuration.GetSection("MongoDB").GetSection("UserDatabase").Value);
        }
        //Define a MongoCollection to represent the Users collection of MongoDB based on UserProfile type

        public IMongoCollection<UserProfile> Users => mongoDb.GetCollection<UserProfile>("users");
    }
}
