using Shared.DTOS.LectureDTOs;
using Shared.DTOS.Common;
using Microsoft.AspNetCore.Http;

namespace BLL.ServiceAbstraction
{
    public interface ILectureService
    {
        // Lecture Management
        Task<List<LectureDTO>> GetAllLecturesAsync();
        Task<List<LectureDTO>> GetPublishedLecturesAsync();
        Task<LectureDTO> GetLectureByIdAsync(int id);
        Task<LectureDTO> CreateLectureAsync(string adminId, CreateLectureDTO createLectureDto);
        Task<LectureDTO> UpdateLectureAsync(int id, UpdateLectureDTO updateLectureDto);
        Task<bool> DeleteLectureAsync(int id);
        Task<LectureDTO> PublishLectureAsync(int id);
        Task<LectureDTO> UnpublishLectureAsync(int id);
        Task<LectureDTO> UploadVideoAsync(string adminId, LectureUploadDTO uploadDto);

        // File Upload Management
        Task<bool> DeleteVideoAsync(int lectureId);
        Task<string> GetVideoStreamUrlAsync(int lectureId);

        // Search and Filter
        Task<PaginatedResponse<LectureDTO>> SearchLecturesAsync(LectureSearchDTO searchDto);
        Task<List<LectureDTO>> GetLecturesByConsultationAsync(int consultationId);
        Task<List<LectureDTO>> GetLecturesByTypeAsync(LectureType type);
        Task<List<LectureDTO>> GetRelatedLecturesAsync(int lectureId);

        // Statistics and Analytics
        Task<bool> IncrementViewCountAsync(int lectureId);
        Task<bool> IncrementDownloadCountAsync(int lectureId);
        Task<object> GetLectureStatisticsAsync();
        Task<object> GetLectureStatisticsByConsultationAsync(int consultationId);

        // Thumbnail Management
        Task<string> GenerateThumbnailAsync(int lectureId);
        Task<bool> UpdateThumbnailAsync(int lectureId, string thumbnailUrl);

        // Validation
        Task<bool> ValidateExternalUrlAsync(string url);
    }
} 