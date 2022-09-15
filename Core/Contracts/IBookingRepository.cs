using Core.Entities;
using Core.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Contracts
{
    public interface IBookingRepository
    {
        Task<int> GetCountAsync();
        Task InsertAsync(Booking newBooking);
        Task DeleteAsync(Booking booking);
        Task UpdateAsync(Booking updateBooking);
    }
}
