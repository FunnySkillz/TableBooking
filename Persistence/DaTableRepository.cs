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

        /// <summary>
        /// Input. First and last name
        /// will get all tables booked - matching the input.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        public async Task<List<DaTable?>> GetTablesByPerson(string firstName, string lastName)
        {
            return (await _dbContext.Bookings
                .Include(b => b.Table)
                .Include(b => b.Person)
                .ToListAsync())
                .Where(b => b.Person!.FirstName.Equals(firstName) && b.Person.LastName.Equals(lastName))
                .GroupBy(b => b.Table)
                .Select(g => g.Key).ToList();
        }
    }
}