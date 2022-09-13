using System.Collections.Generic;
using Core.Contracts;
using Microsoft.EntityFrameworkCore;
using Core.Entities;
using Core.Validations;

namespace Persistence
{
    internal class DaTableRepository : IDaTableRepository
    {
        private ApplicationDbContext _dbContext;

        public DaTableRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(DaTable newTable)
        {
            await _dbContext.AddAsync(newTable);
        }

        public async Task<int> CountAsync()
        {
            return await _dbContext.Tables.CountAsync();
        }

        public async Task<List<DaTable>> GetAllAsync()
        {
            return await _dbContext.Tables.OrderBy(tt => tt.TableNumber).ToListAsync();
        }
    }
}