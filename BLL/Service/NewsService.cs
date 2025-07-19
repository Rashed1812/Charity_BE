using AutoMapper;
using BLL.ServiceAbstraction;
using DAL.Repositories.RepositoryIntrfaces;
using DAL.Data.Models;
using Shared.DTOS.NewsDTOs;
using BLL.Services.FileService;

namespace BLL.Service
{
    public class NewsService : INewsService
    {
        private readonly INewsItemRepository _newsItemRepository;
        private readonly IMapper _mapper;

        public NewsService(INewsItemRepository newsItemRepository, IMapper mapper)
        {
            _newsItemRepository = newsItemRepository;
            _mapper = mapper;
        }

        public async Task<List<NewsItemDTO>> GetAllNewsAsync()
        {
            var news = await _newsItemRepository.GetAllAsync();
            return _mapper.Map<List<NewsItemDTO>>(news);
        }

        public async Task<List<NewsItemDTO>> GetActiveNewsAsync()
        {
            var news = await _newsItemRepository.GetActiveNewsAsync();
            return _mapper.Map<List<NewsItemDTO>>(news);
        }

        public async Task<NewsItemDTO> GetNewsByIdAsync(int id)
        {
            var news = await _newsItemRepository.GetByIdAsync(id);
            return _mapper.Map<NewsItemDTO>(news);
        }

        public async Task<NewsItemDTO> CreateNewsAsync(string adminId, CreateNewsItemDTO createNewsDto)
        {
            var news = _mapper.Map<NewsItem>(createNewsDto);
            news.Author = adminId; // This should be the admin's name, not ID
            news.CreatedAt = DateTime.UtcNow;

            if (news.IsPublished)
                news.PublishedAt = DateTime.UtcNow;

            FileService fs = new FileService();

            var imgUrl =await fs.UploadFileAsync(createNewsDto.Image, fs._newsFileName);
            news.ImageUrl = imgUrl;

            var createdNews = await _newsItemRepository.AddAsync(news);
            return _mapper.Map<NewsItemDTO>(createdNews);
        }

        public async Task<NewsItemDTO> UpdateNewsAsync(int id, UpdateNewsItemDTO updateNewsDto)
        {
            var news = await _newsItemRepository.GetByIdAsync(id);
            if (news == null)
                return null;

            // Update properties
            if (!string.IsNullOrEmpty(updateNewsDto.Title))
                news.Title = updateNewsDto.Title;

            if (!string.IsNullOrEmpty(updateNewsDto.Content))
                news.Content = updateNewsDto.Content;

            if (!string.IsNullOrEmpty(updateNewsDto.Summary))
                news.Summary = updateNewsDto.Summary;

            if (!string.IsNullOrEmpty(updateNewsDto.Category))
                news.Category = updateNewsDto.Category;

            if (updateNewsDto.IsPublished.HasValue)
            {
                news.IsPublished = updateNewsDto.IsPublished.Value;
                if (news.IsPublished && !news.PublishedAt.HasValue)
                    news.PublishedAt = DateTime.UtcNow;
            }

            if (updateNewsDto.Tags != null)
                news.Tags = updateNewsDto.Tags;

            news.UpdatedAt = DateTime.UtcNow;

            if(updateNewsDto.Image!=null)
            {
                FileService fs = new FileService();
                fs.DeleteFile(news.ImageUrl);

                var imgUrl =await fs.UploadFileAsync(updateNewsDto.Image, fs._newsFileName);
                news.ImageUrl = imgUrl;
            }

            var updatedNews = await _newsItemRepository.UpdateAsync(news);
            return _mapper.Map<NewsItemDTO>(updatedNews);
        }

        public async Task<bool> DeleteNewsAsync(int id)
        {
            var news = await _newsItemRepository.GetByIdAsync(id);
            if (news == null)
                return false;
            FileService fs = new FileService();
            await _newsItemRepository.DeleteAsync(id);
            fs.DeleteFile(news.ImageUrl);
            return true;
        }

        public async Task<bool> IncrementViewCountAsync(int id)
        {
            var news = await _newsItemRepository.GetByIdAsync(id);
            if (news == null)
                return false;

            news.ViewCount++;
            await _newsItemRepository.UpdateAsync(news);
            return true;
        }

        public async Task<object> GetNewsStatisticsAsync()
        {
            var news = await _newsItemRepository.GetAllAsync();
            
            return new
            {
                TotalNews = news.Count(),
                PublishedNews = news.Count(n => n.IsPublished),
                TotalViews = news.Sum(n => n.ViewCount),
                MostViewedNews = await _newsItemRepository.GetMostViewedNewsAsync(5)
            };
        }
    }
} 