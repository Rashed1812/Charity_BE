using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositries.GenericRepositries
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> Update(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        void Delete(TEntity entity);
        Task<bool> DeleteAsync(object id);
        Task<int> SaveChangesAsync();
    }
}
