using Core.Contracts;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

        public async void OnGet()
        {
            Persons = await _unitOfWork.PersonRepository.GetAllOrderedByLastNameAsync();
            DaTables = await _unitOfWork.TableRepository.GetAllAsync();
        }

    }
}