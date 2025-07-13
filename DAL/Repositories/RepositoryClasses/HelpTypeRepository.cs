using DAL.Data;
using DAL.Data.Models;
using DAL.Repositories.RepositoryIntrfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories.RepositoryClasses
{
    public class HelpTypeRepository : IHelpTypeRepository
    {
        private readonly ApplicationDbContext _context;
        public HelpTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<HelpType>> GetAllAsync()
        {
            return await _context.HelpTypes.ToListAsync();
        }

        public async Task<HelpType> GetByIdAsync(int id)
        {
            return await _context.HelpTypes.FindAsync(id);
        }

        public async Task<HelpType> AddAsync(HelpType entity)
        {
            _context.HelpTypes.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<HelpType> UpdateAsync(HelpType entity)
        {
            _context.HelpTypes.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(HelpType entity)
        {
            _context.HelpTypes.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
} 