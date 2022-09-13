using Core.Contracts;
using Core.Entities;
using Core.Validations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class BookingsRepository : IBookingRepository
    {
        ApplicationDbContext _dbContext;

        public BookingsRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task DeleteAsync(Booking booking)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetCountAsync()
        {
            return await _dbContext.Bookings.CountAsync();
        }

        public Task InsertAsync(Booking newBooking)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Booking updateBooking)
        {
            throw new NotImplementedException();
        }

        public Task<List<Table>> GetTablesByPerson(string firstName, string lastName)
        {
            throw new NotImplementedException();
        }
    }
}
