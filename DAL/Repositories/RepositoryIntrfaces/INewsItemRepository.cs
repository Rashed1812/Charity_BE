using DAL.Data.Models;
using DAL.Repositries.GenericRepositries;

namespace DAL.Repositories.RepositoryIntrfaces
{
    public interface INewsItemRepository : IGenericRepository<NewsItem>
    {
        Task<List<NewsItem>> GetActiveNewsAsync();
        Task<List<NewsItem>> GetByCategoryAsync(string category);
        Task<List<NewsItem>> GetPublishedNewsAsync();
        Task<List<NewsItem>> GetByAuthorAsync(string author);
        Task<int> GetTotalViewCountAsync();
        Task<List<NewsItem>> GetMostViewedNewsAsync(int count);
    }
} 