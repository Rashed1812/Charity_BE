using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data;
using DAL.Repositries.GenericRepositries;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.GenericRepositries
{
    public class GenericRepository<TEntity>(ApplicationDbContext _dbContext) : IGenericRepository<TEntity> where TEntity : class
    {
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var result = await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }

        public void Delete(TEntity entity) => _dbContext.Set<TEntity>().Remove(entity);

        public async Task<bool> DeleteAsync(object id)
        {
            var entity = await _dbContext.Set<TEntity>().FindAsync(id);
            if (entity == null) return false;
            _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<int> SaveChangesAsync() 
        {
            return await _dbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
