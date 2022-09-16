using Core.Contracts;
using Core.Dtos;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace WebAPP.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(ILogger<IndexModel> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public List<Person> Persons { get; set; } = new List<Person>(); //init with empty list
        public List<DaTable> DaTables { get; set; } = new List<DaTable>();
        public List<Booking> Bookings { get; set; } = new List<Booking>();
        public List<PersonTableSummary> PersonTableSummaries { get; set; } = new List<PersonTableSummary>();

        [BindProperty]
        public string SearchPerson { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            Persons = await _unitOfWork.PersonRepository.GetAllOrderedByLastNameAsync();
            DaTables = await _unitOfWork.TableRepository.GetAllAsync();

            PersonTableSummaries = await _unitOfWork.BookingRepository.PersonTableSummaryAsync();
        }

    }
}