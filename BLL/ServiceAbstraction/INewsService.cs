using Shared.DTOS.NewsDTOs;

namespace BLL.ServiceAbstraction
{
    public interface INewsService
    {
        // News Management
        Task<List<NewsItemDTO>> GetAllNewsAsync();
        Task<List<NewsItemDTO>> GetActiveNewsAsync();
        Task<NewsItemDTO> GetNewsByIdAsync(int id);
        Task<NewsItemDTO> CreateNewsAsync(string adminId, CreateNewsItemDTO createNewsDto);
        Task<NewsItemDTO> UpdateNewsAsync(int id, UpdateNewsItemDTO updateNewsDto);
        Task<bool> DeleteNewsAsync(int id);
        Task<bool> IncrementViewCountAsync(int id);

        // Statistics
        Task<object> GetNewsStatisticsAsync();
    }
} 