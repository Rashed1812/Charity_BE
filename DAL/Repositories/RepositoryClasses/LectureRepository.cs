using DAL.Data;
using DAL.Data.Models;
using DAL.Repositories.GenericRepositries;
using DAL.Repositories.RepositoryIntrfaces;
using Microsoft.EntityFrameworkCore;
using Shared.DTOS.LectureDTOs;

namespace DAL.Repositories.RepositoryClasses
{
    public class LectureRepository : GenericRepository<Lecture>, ILectureRepository
    {
        private readonly ApplicationDbContext _context;
        public LectureRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Lecture>> GetPublishedLecturesAsync()
        {
            return await _context.Lectures
                .Include(l => l.Consultation)
                .Include(l => l.CreatedByUser)
                .Where(l => l.IsPublished)
                .OrderByDescending(l => l.PublishedAt)
                .ToListAsync();
        }

        public async Task<List<Lecture>> GetByConsultationIdAsync(int consultationId)
        {
            return await _context.Lectures
                .Include(l => l.Consultation)
                .Include(l => l.CreatedByUser)
                .Where(l => l.ConsultationId == consultationId && l.IsPublished)
                .OrderByDescending(l => l.PublishedAt)
                .ToListAsync();
        }

        public async Task<List<Lecture>> GetByTypeAsync(LectureType type)
        {
            return await _context.Lectures
                .Include(l => l.Consultation)
                .Include(l => l.CreatedByUser)
                .Where(l => l.Type == type && l.IsPublished)
                .OrderByDescending(l => l.PublishedAt)
                .ToListAsync();
        }

        public async Task<List<Lecture>> GetBySpeakerAsync(string speaker)
        {
            return await _context.Lectures
                .Include(l => l.Consultation)
                .Include(l => l.CreatedByUser)
                .Where(l => l.Speaker == speaker && l.IsPublished)
                .OrderByDescending(l => l.PublishedAt)
                .ToListAsync();
        }

        public async Task<List<Lecture>> GetMostViewedLecturesAsync(int count)
        {
            return await _context.Lectures
                .Include(l => l.Consultation)
                .Include(l => l.CreatedByUser)
                .Where(l => l.IsPublished)
                .OrderByDescending(l => l.ViewCount)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<Lecture>> SearchLecturesAsync(string searchTerm)
        {
            return await _context.Lectures
                .Include(l => l.Consultation)
                .Include(l => l.CreatedByUser)
                .Where(l => l.IsPublished && 
                           (l.Title.Contains(searchTerm) || 
                            l.Description.Contains(searchTerm) || 
                            l.Speaker.Contains(searchTerm)))
                .OrderByDescending(l => l.PublishedAt)
                .ToListAsync();
        }

        public async Task<int> GetTotalViewCountAsync()
        {
            return await _context.Lectures
                .SumAsync(l => l.ViewCount);
        }

        public async Task<int> GetTotalDownloadCountAsync()
        {
            return await _context.Lectures
                .SumAsync(l => l.DownloadCount);
        }

        public async Task<List<Lecture>> GetLecturesByConsultationAsync(int consultationId)
        {
            return await _context.Lectures
                .Include(l => l.Consultation)
                .Include(l => l.CreatedByUser)
                .Where(l => l.ConsultationId == consultationId && l.IsPublished)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Lecture>> GetLecturesByTypeAsync(LectureType type)
        {
            return await _context.Lectures
                .Include(l => l.Consultation)
                .Include(l => l.CreatedByUser)
                .Where(l => l.Type == type && l.IsPublished)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Lecture>> GetLecturesByAdminAsync(string adminId)
        {
            return await _context.Lectures
                .Include(l => l.Consultation)
                .Include(l => l.CreatedByUser)
                .Where(l => l.CreatedBy == adminId)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Lecture>> GetRelatedLecturesAsync(int lectureId)
        {
            var lecture = await _context.Lectures
                .Include(l => l.Consultation)
                .FirstOrDefaultAsync(l => l.Id == lectureId);

            if (lecture == null)
                return new List<Lecture>();

            return await _context.Lectures
                .Include(l => l.Consultation)
                .Include(l => l.CreatedByUser)
                .Where(l => l.IsPublished && 
                           (l.ConsultationId == lecture.ConsultationId || 
                            l.Tags.Contains(lecture.Tags) ||
                            l.Id != lectureId))
                .OrderByDescending(l => l.ViewCount)
                .Take(5)
                .ToListAsync();
        }

        public async Task<List<Lecture>> SearchLecturesAsync(string searchTerm, int? consultationId, LectureType? type, bool? isPublished)
        {
            var query = _context.Lectures
                .Include(l => l.Consultation)
                .Include(l => l.CreatedByUser)
                .Where(l => l.IsPublished);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(l => l.Title.Contains(searchTerm) || 
                                        l.Description.Contains(searchTerm) || 
                                        l.Tags.Contains(searchTerm));
            }

            if (consultationId.HasValue)
            {
                query = query.Where(l => l.ConsultationId == consultationId.Value);
            }

            if (type.HasValue)
            {
                query = query.Where(l => l.Type == type.Value);
            }

            if (isPublished.HasValue)
            {
                query = query.Where(l => l.IsPublished == isPublished.Value);
            }

            return await query
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetTotalLecturesCountAsync()
        {
            return await _context.Lectures.CountAsync(l => l.IsPublished);
        }

        public async Task<int> GetPublishedLecturesCountAsync()
        {
            return await _context.Lectures.CountAsync(l => l.IsPublished);
        }

        public async Task<int> GetLecturesCountByConsultationAsync(int consultationId)
        {
            return await _context.Lectures.CountAsync(l => l.ConsultationId == consultationId && l.IsPublished);
        }

        public async Task<int> GetLecturesCountByTypeAsync(LectureType type)
        {
            return await _context.Lectures.CountAsync(l => l.Type == type && l.IsPublished);
        }

        public async Task<bool> IncrementViewCountAsync(int lectureId)
        {
            var lecture = await _context.Lectures.FindAsync(lectureId);
            if (lecture == null)
                return false;

            lecture.ViewCount++;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IncrementDownloadCountAsync(int lectureId)
        {
            var lecture = await _context.Lectures.FindAsync(lectureId);
            if (lecture == null)
                return false;

            lecture.DownloadCount++;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PublishLectureAsync(int lectureId)
        {
            var lecture = await _context.Lectures.FindAsync(lectureId);
            if (lecture == null)
                return false;

            lecture.IsPublished = true;
            lecture.Status = LectureStatus.Published;
            lecture.PublishedAt = DateTime.UtcNow;
            lecture.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnpublishLectureAsync(int lectureId)
        {
            var lecture = await _context.Lectures.FindAsync(lectureId);
            if (lecture == null)
                return false;

            lecture.IsPublished = false;
            lecture.Status = LectureStatus.Draft;
            lecture.PublishedAt = null;
            lecture.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateThumbnailAsync(int lectureId, string thumbnailUrl)
        {
            var lecture = await _context.Lectures.FindAsync(lectureId);
            if (lecture == null)
                return false;

            lecture.ThumbnailUrl = thumbnailUrl;
            lecture.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Lecture>> GetAllLecturesWithAuthorAsync()
        {
            return await _context.Lectures
                .Include(l => l.CreatedByUser)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Lecture>> GetPublishedLecturesWithAuthorAsync()
        {
            return await _context.Lectures
                .Include(l => l.CreatedByUser)
                .Where(l => l.IsPublished)
                .OrderByDescending(l => l.PublishedAt)
                .ToListAsync();
        }

        public async Task<Lecture> GetLectureByIdWithAuthorAsync(int id)
        {
            return await _context.Lectures
                .Include(l => l.CreatedByUser)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<List<Lecture>> GetLecturesByAuthorAsync(string authorId)
        {
            return await _context.Lectures
                .Where(l => l.CreatedBy == authorId)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Lecture>> GetLecturesByStatusAsync(LectureStatus status)
        {
            return await _context.Lectures
                .Include(l => l.CreatedByUser)
                .Where(l => l.Status == status)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetLecturesCountByStatusAsync(LectureStatus status)
        {
            return await _context.Lectures.CountAsync(l => l.Status == status);
        }

        public async Task<List<object>> GetLecturesByMonthAsync(int months)
        {
            var startDate = DateTime.UtcNow.AddMonths(-months);
            return await _context.Lectures
                .Where(l => l.CreatedAt >= startDate)
                .GroupBy(l => new { l.CreatedAt.Year, l.CreatedAt.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .ToListAsync<object>();
        }

        public async Task<List<object>> GetLecturesByAuthorCountAsync()
        {
            return await _context.Lectures
                .GroupBy(l => l.CreatedBy)
                .Select(g => new
                {
                    AuthorId = g.Key,
                    AuthorName = g.First().CreatedByUser.FullName,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToListAsync<object>();
        }

        public async Task<int> GetTotalViewsAsync()
        {
            return await _context.Lectures.SumAsync(l => l.ViewCount);
        }

        public async Task<bool> HasLecturesAsync(string authorId)
        {
            return await _context.Lectures.AnyAsync(l => l.CreatedBy == authorId);
        }

        public async Task<Lecture> GetByIdWithRelatedData(int id)
        {
            return await _context.Lectures
                .Include(l => l.Consultation)
                .Include(l => l.CreatedByUser)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<List<Lecture>> GetAllWithRelatedDataAsync()
        {
            return await _context.Lectures
                .Include(l => l.Consultation)
                .Include(l => l.CreatedByUser)
                .ToListAsync();
        }
    }
} 