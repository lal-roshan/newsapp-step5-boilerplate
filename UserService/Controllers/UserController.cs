using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserService.Exceptions;
using UserService.Models;
using UserService.Services;
namespace UserService.Controllers
{
    /*
     * As in this assignment, we are working with creating RESTful web service, hence annotate
     * the class with [ApiController] annotation and define the controller level route as per 
     * REST Api standard.
     * 
     */
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        readonly IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        /* Implement HttpVerbs and its Functionality asynchronously*/

        /*
         * Define a handler method which will get us the user by a userId.
         * This handler method should return any one of the status messages basis on
         * different situations: 
         * 1. 200(OK) - If the news found successfully.
         * This handler method should map to the URL "/api/user/{userId}" using HTTP GET method
         */
        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(string userId)
        {
            try
            {
                return Ok(await userService.GetUser(userId));
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Some error occurred, please try again!!");
            }
        }

        /*
        * Define a handler method which will create a specific UserProfile by reading the
        * Serialized object from request body and save the user details in a User table
        * in the database.
        * 
        * Please note that AddUser method should add a userdetails and also handle the exception using 
        * ExceptionHandler.This handler method should return any one of the status messages 
        * basis on different situations: 
        * 1. 201(CREATED) - If the userProfile details created successfully. 
        * 2. 409(CONFLICT) - If the userId conflicts with any existing userId
        * 
        * This handler method should map to the URL "/api/user" using HTTP POST method
        */
        [HttpPost]
        public async Task<IActionResult> Post(UserProfile user)
        {
            try
            {
                return Ok(await userService.AddUser(user));
            }
            catch (UserAlreadyExistsException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Some error occurred, please try again!!");
            }
        }


        /*
        * Define a handler method which will update a specific user by reading the
        * Serialized object from request body and save the updated user details in a
        * user table in database handle exception as well.
        * This handler method should return any one of the status messages basis on different situations: 
        * 1. 200(OK) - If the user updated successfully. 
        * 2. 404(NOT FOUND) - If the user with specified userId is not found. 
        * 
        * This handler method should map to the URL "/api/user/{userId}" using HTTP PUT method.
        */
        [HttpPut("{userId}")]
        public async Task<IActionResult> Put(string userId, UserProfile user)
        {
            try
            {
                return Ok(await userService.UpdateUser(userId, user));
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Some error occurred, please try again!!");
            }
        }

        /*
             * Define a handler method which will delete a specified UserProfile details from a database.
             * This handler method should return any one of the status messages basis on
             * different situations: 
             * 1. 200(OK) - If the userProfile deleted successfully from database. 
             * 2. 404(NOT FOUND) - If the userProfile details with specified userId is not found.
             * This handler method should map to the URL "/api/user/{userId}" using HTTP Delete
             * method" where "id" should be replaced by a valid userId without {}
        */
        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete(string userId)
        {
            try
            {
                return Ok(await userService.DeleteUser(userId));
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Some error occurred, please try again!!");
            }
        }
    }
}
