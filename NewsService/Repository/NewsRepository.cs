using MongoDB.Driver;
using MongoDB.Driver.Linq;
using NewsService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace NewsService.Repository
{
    //Inherit the respective interface and implement the methods in
    // this class i.e NewsRepository by inheriting INewsRepository
    public class NewsRepository : INewsRepository
    {
        //define a private variable to represent NewsDbContext
        readonly NewsContext newsContext;

        public NewsRepository(NewsContext newsContext)
        {
            this.newsContext = newsContext;
        }

        public async Task<bool> AddOrUpdateReminder(string userId, int newsId, Reminder reminder)
        {
            var builder = Builders<UserNews>.Filter;
            var filter = builder.Eq(u => u.UserId, userId)
                & builder.ElemMatch(u => u.NewsList, n => n.NewsId == newsId);
            var update = Builders<UserNews>.Update.Set(u => u.NewsList.SingleOrDefault().Reminder, reminder);
            var result = await newsContext.UserNews.UpdateOneAsync(filter,update, new UpdateOptions { IsUpsert = true});
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<int> CreateNews(string userId, News news)
        {
            int newsId = 101;
            var filter = Builders<UserNews>.Filter.Eq(u => u.UserId, userId);
            var userNewsResult = await newsContext.UserNews.FindAsync(filter);
            var userNews = await userNewsResult.FirstOrDefaultAsync();
            if(userNews != null && userNews.NewsList != null && userNews.NewsList.Any())
            {
                newsId = userNews.NewsList.Max(n => n.NewsId) + 1;
            }
            news.NewsId = newsId;
            var update = Builders<UserNews>.Update.Push(u => u.NewsList, news);
            var result = await newsContext.UserNews.UpdateOneAsync(filter, update);
            if(result.IsAcknowledged && result.ModifiedCount > 0)
            {
                return newsId;
            }
            return -1;
        }

        public async Task<bool> DeleteNews(string userId, int newsId)
        {
            var builder = Builders<UserNews>.Filter;
            var filter = builder.Eq(u => u.UserId, userId) & builder.AnyEq(u => u.NewsList.Select(n => n.NewsId), newsId);
            var update = Builders<UserNews>.Update.Unset(u => u.NewsList.SingleOrDefault());
            var result = await newsContext.UserNews.UpdateOneAsync(filter, update);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteReminder(string userId, int newsId)
        {
            var builder = Builders<UserNews>.Filter;
            var filter = builder.Eq(u => u.UserId, userId) & builder.AnyEq(u => u.NewsList.Select(n => n.NewsId), newsId);
            var update = Builders<UserNews>.Update.Unset(u => u.NewsList.SingleOrDefault().Reminder);
            var result = await newsContext.UserNews.UpdateOneAsync(filter, update);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<List<News>> FindAllNewsByUserId(string userId)
        {
            var result = await newsContext.UserNews.FindAsync(u => string.Equals(userId, u.UserId));
            if (await result.AnyAsync())
            {
                return result.Current.FirstOrDefault().NewsList;
            }
            return null;
        }

        public async Task<News> GetNewsById(string userId, int newsId)
        {
            var builder = Builders<UserNews>.Filter;
            var filter = builder.Eq(u => u.UserId, userId)
                & builder.ElemMatch(u => u.NewsList, n => n.NewsId == newsId);
            var result = await newsContext.UserNews.FindAsync(filter);
            if (await result.AnyAsync())
            {
                var userNews = await result.SingleOrDefaultAsync();
                return userNews?.NewsList?.Count > 1 ? null
                    : userNews?.NewsList?.SingleOrDefault();
            }
            return null;
        }

        public async Task<bool> IsNewsExist(string userId, string title)
        {
            var builder = Builders<UserNews>.Filter;
            var filter = builder.Eq(u => u.UserId, userId)
                & builder.ElemMatch(u => u.NewsList, n => string.Equals(n.Title, title));
            var news = await newsContext.UserNews.FindAsync(filter);
            if (news.Any())
            {
                return true;
            }
            return false;
        }

        public async Task<bool> IsReminderExists(string userId, int newsId)
        {
            var news = await GetNewsById(userId, newsId);
            if (news != null && news.Reminder != null)
            {
                return true;
            }
            return false;
        }
        /* Implement all the methods of respective interface asynchronously*/

        // CreateNews method should be used to create a new news. NewsId should be autogenerated and
        // must start with 101.This should create a new UserNews if not exists else should push 
        //new news entry into existing UserNews collection.


        //FindAllNewsByUserId method should be used to retreive all news for a user by userId

        //DeleteNews method should be used to delete a news for a specific user

        //IsNewsExist method is used to check news of individual userId exist or not

        // GetNewsById  method is used to get the news by userId

        //AddOrUpdateReminder method is used to Add and update the reminder by userId and newsId

        //Delete Reminder method is used to Delete the created Reminder by userId

        //IsReminderExists method is used to check the Reminder Exist or not by userId

    }
}
