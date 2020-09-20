using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
namespace ReminderService.Models
{
    public class ReminderContext
    {
        //declare variables to connect to MongoDB database
        readonly MongoClient mongoClient;

        readonly IMongoDatabase mongoDb;

        public ReminderContext(IConfiguration configuration)
        {
            //Initialize MongoClient and Database using connection string and database name from configuration
            mongoClient = new MongoClient(configuration.GetSection("MongoDB").GetSection("ConnectionString").Value);
            mongoDb = mongoClient.GetDatabase(configuration.GetSection("MongoDB").GetSection("ReminderDatabase").Value);
        }
        //Define a MongoCollection to represent the News collection of MongoDB based on UserNews type
        public IMongoCollection<Reminder> Reminders => mongoDb.GetCollection<Reminder>("reminders");
    }
}