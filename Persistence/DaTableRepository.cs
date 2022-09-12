using System.Collections.Generic;
using Core.Contracts;
using Microsoft.EntityFrameworkCore;
using Core.Entities;

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

        public async Task<List<DaTable>> GetAllAsync(int? person_id)
        {
            return await _dbContext.Tables.AsNoTracking<DaTable>().Where(r => !person_id.HasValue || r.Person_Id == person_id).OrderBy(r => r.TableName).ToListAsync();
        }

        public async Task<List<DaTable>> GetAll()
        {
            return await _dbContext.Tables.OrderBy(tt => tt.TableName).ToListAsync();
        }

        public async Task<List<DaTable>> GetTablesByPersonNameAsync(string firstName, string lastName)
        {
            return await _dbContext.Tables.Where(r => r.Person!.FirstName.ToUpper() == firstName.ToUpper() && r.Person.LastName.ToUpper() == lastName.ToUpper())
                .OrderBy(r => r.TableName).ThenBy(r => r.QRCode).ToListAsync();
        }


    }
}