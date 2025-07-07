using DAL.Data.Models;
using DAL.Repositries.GenericRepositries;
using Shared.DTOS.LectureDTOs;

namespace DAL.Repositories.RepositoryIntrfaces
{
    public interface ILectureRepository : IGenericRepository<Lecture>
    {
        // Basic CRUD operations inherited from IGenericRepository
        
        // Custom queries
        Task<List<Lecture>> GetPublishedLecturesAsync();
        Task<List<Lecture>> GetByConsultationIdAsync(int consultationId);
        Task<List<Lecture>> GetByTypeAsync(LectureType type);
        Task<List<Lecture>> GetBySpeakerAsync(string speaker);
        Task<List<Lecture>> GetMostViewedLecturesAsync(int count);
        Task<List<Lecture>> SearchLecturesAsync(string searchTerm);
        Task<int> GetTotalViewCountAsync();
        Task<int> GetTotalDownloadCountAsync();
        
        // Statistics
        Task<int> GetTotalLecturesCountAsync();
        Task<int> GetPublishedLecturesCountAsync();
        Task<int> GetLecturesCountByConsultationAsync(int consultationId);
        Task<int> GetLecturesCountByTypeAsync(LectureType type);
        
        // File operations
        Task<bool> UpdateVideoFilePathAsync(int lectureId, string filePath, string fileName, long fileSize, string contentType);
        Task<bool> DeleteVideoFileAsync(int lectureId);
        
        // View and download tracking
        Task<bool> IncrementViewCountAsync(int lectureId);
        Task<bool> IncrementDownloadCountAsync(int lectureId);
        
        // Publishing
        Task<bool> PublishLectureAsync(int lectureId);
        Task<bool> UnpublishLectureAsync(int lectureId);
        
        // Thumbnail operations
        Task<bool> UpdateThumbnailAsync(int lectureId, string thumbnailUrl);

        Task<List<Lecture>> GetAllLecturesWithAuthorAsync();
        Task<List<Lecture>> GetPublishedLecturesWithAuthorAsync();
        Task<Lecture> GetLectureByIdWithAuthorAsync(int id);
        Task<List<Lecture>> GetLecturesByAuthorAsync(string authorId);
        Task<List<Lecture>> GetLecturesByStatusAsync(LectureStatus status);
        Task<int> GetLecturesCountByStatusAsync(LectureStatus status);
        Task<List<object>> GetLecturesByMonthAsync(int months);
        Task<List<object>> GetLecturesByAuthorCountAsync();
        Task<int> GetTotalViewsAsync();
        Task<bool> HasLecturesAsync(string authorId);
    }
} 