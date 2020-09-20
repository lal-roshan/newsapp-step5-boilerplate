using ReminderService.Exceptions;
using ReminderService.Models;
using ReminderService.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ReminderService.Services
{
    public class ReminderService : IReminderService
    {
        //Inherit the respective interface and implement the methods in 
        // this class i.e ReminderService by inheriting IReminderService

        /*
      * ReminderRepository should  be injected through constructor injection. 
      * Please note that we should not create ReminderRepository object using the new keyword
      */
        readonly IReminderRepository reminderRepository;

        public ReminderService(IReminderRepository reminderRepository)
        {
            this.reminderRepository = reminderRepository;
        }

        public async Task<bool> CreateReminder(string userId, string email, ReminderSchedule schedule)
        {
            if (!await reminderRepository.IsReminderExists(userId, schedule.NewsId))
            {
                await reminderRepository.CreateReminder(userId, email, schedule);
                return await reminderRepository.IsReminderExists(userId, schedule.NewsId);
            }
            else
            {
                throw new ReminderAlreadyExistsException($"This News already have a reminder");
            }
        }

        public async Task<bool> DeleteReminder(string userId, int newsId)
        {
            if(await reminderRepository.IsReminderExists(userId, newsId))
            {
                return await reminderRepository.DeleteReminder(userId, newsId);
            }
            else
            {
                throw new NoReminderFoundException("No reminder found for this news");
            }
        }

        public async Task<List<ReminderSchedule>> GetReminders(string userId)
        {
            var reminders = await reminderRepository.GetReminders(userId);
            if(reminders != null && reminders.Any())
            {
                return reminders;
            }
            else
            {
                throw new NoReminderFoundException("No reminders found for this user");
            }
        }

        public async Task<bool> UpdateReminder(string userId, ReminderSchedule reminder)
        {
            if(await reminderRepository.IsReminderExists(userId, reminder.NewsId))
            {
                return await reminderRepository.UpdateReminder(userId, reminder);
            }
            else
            {
                throw new NoReminderFoundException("No reminder found for this news");
            }
        }

        /* Implement all the methods of respective interface asynchronously*/


        // Implement GetReminders method which should be used to get all reminders by userId.

        // Implement CreateReminder method which should be used to create a new reminder.   

        // Implement DeleteReminder method which should be used to delete a reminder by userId and newsId

        // Implement a UpdateReminder method which should be used to update an existing reminder by using
        // userId and reminder details  


        // Throw your own custom Exception whereever its required in  GetReminders, CreateReminder, DeleteReminder, 
        // and UpdateReminder functionalities
    }
}
