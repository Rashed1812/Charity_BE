using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data;
using DAL.Repositries.GenericRepositries;

namespace DAL.Repositories.GenericRepositries
{
    public class GenericRepository<TEntity>(ApplicationDbContext _dbContext) : IGenericRepository<TEntity> where TEntity : class
    {
        public async Task AddAsync(TEntity entity) => await _dbContext.Set<TEntity>().AddAsync(entity);

        public void Delete(TEntity entity) => _dbContext.Set<TEntity>().Remove(entity);

        public async Task<int> SaveChangesAsync() 
        {
            return await _dbContext.SaveChangesAsync();
        } 
        public Task<IEnumerable<TEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }


        public void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void UpdateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
