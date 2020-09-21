using NewsService.Models;
using NewsService.Repository;
using NewsService.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace NewsService.Services
{
    //Inherit the respective interface and implement the methods in 
    // this class i.e NewsService by inheriting INewsService

    public class NewsService : INewsService
    {
        /*
        * NewsRepository should  be injected through constructor injection. 
        * Please note that we should not create NewsRepository object using the new keyword
        */
        readonly INewsRepository newsRepository;

        public NewsService(INewsRepository newsRepository)
        {
            this.newsRepository = newsRepository;
        }


        /* Implement all the methods of respective interface asynchronously*/

        /* Implement CreateNews method to add the new news details*/

        /* Implement AddOrUpdateReminder using userId and newsId*/

        /* Implement DeleteNews method to remove the existing news*/

        /* Implement DeleteReminder method to delte the Reminder using userId*/

        /* Implement FindAllNewsByUserId to get the News Details by userId*/
        public async Task<bool> AddOrUpdateReminder(string userId, int newsId, Reminder reminder)
        {
            if(await newsRepository.GetNewsById(userId, newsId) != null)
            {
                return await newsRepository.AddOrUpdateReminder(userId, newsId, reminder);
            }
            else
            {
                throw new NoNewsFoundException($"NewsId {newsId} for {userId} doesn't exist");
            }
        }

        public async Task<int> CreateNews(string userId, News news)
        {
            if(! await newsRepository.IsNewsExist(userId, news.Title))
            {
                return await newsRepository.CreateNews(userId, news);
            }
            else
            {
                throw new NewsAlreadyExistsException($"{userId} have already added this news");
            }
        }

        public async Task<bool> DeleteNews(string userId, int newsId)
        {
            var news = await newsRepository.GetNewsById(userId, newsId);
            if ( news != null)
            {
                return await newsRepository.DeleteNews(userId, newsId);
            }
            else
            {
                throw new NoNewsFoundException($"NewsId {newsId} for {userId} doesn't exist");
            }
        }

        public async Task<bool> DeleteReminder(string userId, int newsId)
        {
            if(await newsRepository.IsReminderExists(userId, newsId))
            {
                return await newsRepository.DeleteReminder(userId, newsId);
            }
            else
            {
                throw new NoReminderFoundException("No reminder found for this news");
            }
        }

        public async Task<List<News>> FindAllNewsByUserId(string userId)
        {
            var newsList = await newsRepository.FindAllNewsByUserId(userId);
            if(newsList != null)
            {
                return newsList;
            }
            else
            {
                throw new NoNewsFoundException($"No news found for {userId}");
            }
        }
    }
}
