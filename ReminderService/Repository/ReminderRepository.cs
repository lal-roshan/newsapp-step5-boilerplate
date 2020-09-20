using MongoDB.Driver;
using ReminderService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ReminderService.Repository
{
    //Inherit the respective interface and implement the methods in 
    // this class i.e ReminderRepository by inheriting IReminderRepository class 
    //which is used to implement all Data access operations
    public class ReminderRepository: IReminderRepository
    {
        //define a private variable to represent Reminder Database Context
        readonly ReminderContext reminderContext;

        public ReminderRepository(ReminderContext reminderContext)
        {
            this.reminderContext = reminderContext;
        }

        public async Task CreateReminder(string userId, string email, ReminderSchedule schedule)
        {
            var builder = Builders<Reminder>.Filter;
            var filter = builder.Eq(r => r.UserId, userId) & builder.Eq(r => r.Email, email);
            var update = Builders<Reminder>.Update.Push(r => r.NewsReminders, schedule);
            await reminderContext.Reminders.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
        }

        public async Task<bool> DeleteReminder(string userId, int newsId)
        {
            var builder = Builders<Reminder>.Filter;
            var pull = Builders<Reminder>.Update.PullFilter(r => r.NewsReminders, n => n.NewsId == newsId);
            var filter = builder.Eq(r => r.UserId, userId)
                & builder.ElemMatch(r => r.NewsReminders, n => n.NewsId == newsId);
            var result = await reminderContext.Reminders.UpdateOneAsync(filter, pull);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<List<ReminderSchedule>> GetReminders(string userId)
        {
            var result = await reminderContext.Reminders.FindAsync(r => string.Equals(r.UserId, userId));
            var reminders = await result.FirstOrDefaultAsync();
            return reminders?.NewsReminders;
        }

        public async Task<bool> IsReminderExists(string userId, int newsId)
        {
            var builder = Builders<Reminder>.Filter;
            var filter = builder.Eq(r => r.UserId, userId);
            var projection = Builders<Reminder>.Projection.ElemMatch(u => u.NewsReminders, n => n.NewsId == newsId);
            var result = await reminderContext.Reminders.FindAsync(filter, new FindOptions<Reminder, Reminder> { Projection = projection});
            if(result != null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateReminder(string userId, ReminderSchedule reminder)
        {
            var builder = Builders<Reminder>.Filter;
            var filter = builder.Eq(r => r.UserId, userId)
                & builder.ElemMatch(r => r.NewsReminders, n => n.NewsId == reminder.NewsId);
            var update = Builders<Reminder>.Update.Set("NewsReminders.$.Schedule",reminder.Schedule);
            var result = await reminderContext.Reminders.UpdateOneAsync(filter, update);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
        //Implement the methods of interface Asynchronously.

        // Implement CreateReminder method which should be used to save a new reminder.  

        // Implement DeleteReminder method which should be used to delete an existing reminder.

        // Implement GetReminders method which should be used to get a reminder by userId.

        // Implement IsReminderExists method which should be used to check an existing reminder by userId

        // Implement UpdateReminder method which should be used to update an existing reminder using  userId and 
        // reminder Schedule

    }
}
