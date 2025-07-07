using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data.Models;
using DAL.Repositries.GenericRepositries;

namespace DAL.Repositories.RepositoryIntrfaces
{
    public interface IConsultationRepository : IGenericRepository<Consultation>
    {
        Task<IEnumerable<Consultation>> GetAllWithIncludesAsync();
        Task<Consultation> GetByIdWithIncludesAsync(int id);
        Task<List<Consultation>> GetActiveConsultationsAsync();
        Task<Consultation> GetByNameAsync(string name);
        Task<int> GetAdvisorCountAsync(int consultationId);
        Task<int> GetRequestCountAsync(int consultationId);
        Task<int> GetLectureCountAsync(int consultationId);
    }
}
