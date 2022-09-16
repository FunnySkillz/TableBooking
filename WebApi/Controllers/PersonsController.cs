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
        [HttpGet("GetPersonOrderedByLastName")]
        public async Task<IActionResult> GetAllOrderedByLastName()
        {
            return Ok(await _unitOfWork.PersonRepository.GetAllOrderedByLastNameAsync());
        }

        [ProducesResponseType(typeof(IEnumerable<Person>), StatusCodes.Status200OK)]
        [HttpGet("GetPersonByFirstLastName")]
        public async Task<IActionResult> GetNutrientsByFoodId(string firstName, string lastName)
        {
            return Ok(await _unitOfWork.PersonRepository.GetPersonByNameAsync(firstName, lastName));
        }

    }
}
