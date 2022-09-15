using Core.Contracts;
using Core.Dtos;
using Core.Entities;
using Core.Validations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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


        public async Task InsertAsync(Booking newBooking)
        {
            await _dbContext.Bookings.AddAsync(newBooking);
        }

        // WRONG
        public Task UpdateAsync(Booking updateBooking)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PersonTableSummary>> PersonTableSummaryAsync()
        {
            return (await _dbContext.Bookings
                .Include(b => b.Table)
                .Include(b => b.Person)
                .ToListAsync())
                .Select(grp =>
                    new PersonTableSummary()
                    {
                        FirstName = grp.Person!.FirstName,
                        LastName = grp.Person.LastName,
                        QRCode = grp.Table!.QRCode,
                        TableNumber = grp.Table.TableNumber
                    })
                .OrderBy(r => r.LastName)
                .ToList();
        }
    }
}
