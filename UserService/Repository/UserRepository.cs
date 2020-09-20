using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models;
namespace UserService.Repository
{
    //Inherit the respective interface and implement the methods in 
    // this class i.e UserRepository by inheriting IUserRepository class 
    //which is used to implement all methods in the classs
    public class UserRepository: IUserRepository
    {
        //define a private variable to represent Reminder Database Context
        readonly UserContext userContext;

        public UserRepository(UserContext userContext)
        {
            this.userContext = userContext;
        }

        public async Task<bool> AddUser(UserProfile user)
        {
            await userContext.Users.InsertOneAsync(user);
            var result = await userContext.Users.FindAsync(u => u.UserId == user.UserId);
            return await result.AnyAsync();
        }

        public async Task<bool> DeleteUser(string userId)
        {
            var filter = Builders<UserProfile>.Filter.Where(u => u.UserId == userId);
            var result = await userContext.Users.DeleteOneAsync(filter);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<UserProfile> GetUser(string userId)
        {
            var result = await userContext.Users.FindAsync(u => u.UserId == userId);
            return await result.FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateUser(UserProfile user)
        {
            var filter = Builders<UserProfile>.Filter.Where(u => u.UserId == user.UserId);
            var update = Builders<UserProfile>.Update
                .Set(u => u.FirstName, user.FirstName)
                .Set(u => u.LastName, user.LastName)
                .Set(u => u.Contact, user.Contact)
                .Set(u => u.Email, user.Email);
            var result = await userContext.Users.UpdateOneAsync(filter, update);

            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        //Implement the methods of interface Asynchronously.

        // Implement AddUser method which should be used to add  a new user Profile.  

        // Implement DeleteUser method which should be used to delete an existing user by userId.


        // Implement GetUser method which should be used to get a user by userId.



        // Implement UpdateUser method which should be used to update an existing user by using
        // UserProfile details.
    }
}
