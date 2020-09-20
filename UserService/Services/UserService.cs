using MongoDB.Driver;
using System.Threading.Tasks;
using UserService.Exceptions;
using UserService.Models;
using UserService.Repository;
namespace UserService.Services
{
    //Inherit the respective interface and implement the methods in 
    // this class i.e UserService by inheriting IUserService
    public class UserService: IUserService
    {
        /*
         * UserRepository should  be injected through constructor injection. 
         * Please note that we should not create USerRepository object using the new keyword
         */
        readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<bool> AddUser(UserProfile user)
        {
            var presentUser = await userRepository.GetUser(user.UserId);
            if(presentUser == null)
            {
                return await userRepository.AddUser(user);
            }
            else
            {
                throw new UserAlreadyExistsException($"{user.UserId} is already in use");
            }
        }

        public async Task<bool> DeleteUser(string userId)
        {
            var presentUser = await userRepository.GetUser(userId);
            if (presentUser != null)
            {
                return await userRepository.DeleteUser(userId);
            }
            else
            {
                throw new UserNotFoundException($"This user id doesn't exist");
            }
        }

        public async Task<UserProfile> GetUser(string userId)
        {
            var presentUser = await userRepository.GetUser(userId);
            if (presentUser != null)
            {
                return presentUser;
            }
            else
            {
                throw new UserNotFoundException($"This user id doesn't exist");
            }
        }

        public async Task<bool> UpdateUser(string userId, UserProfile user)
        {
            var presentUser = await userRepository.GetUser(userId);
            if (presentUser != null)
            {
                return await userRepository.UpdateUser(user);
            }
            else
            {
                throw new UserNotFoundException($"This user id doesn't exist");
            }
        }
        //Implement the methods of interface Asynchronously.

        // Implement AddUser method which should be used to add  a new user Profile.  

        // Implement DeleteUser method which should be used to delete an existing user by userId.


        // Implement GetUser method which should be used to get a user by userId.

        // Implement UpdateUser method which should be used to update an existing user by using
        // UserProfile details.
    }
}
