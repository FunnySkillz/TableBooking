using Core.Contracts;
using Core.Dtos;
using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Produces("application/json")]  
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {

        IUnitOfWork _unitOfWork;

        public PersonsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [ProducesResponseType(typeof(IEnumerable<Person>), StatusCodes.Status200OK)]
        [HttpGet("Person")]
        public async Task<IActionResult> RunSummary()
        {
            return Ok(await _unitOfWork.PersonRepository.GetAllOrderedByLastNameAsync());
        }
    }
}
