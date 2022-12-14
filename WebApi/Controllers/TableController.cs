using Core.Contracts;
using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TableController : ControllerBase
    {
        IUnitOfWork _unitOfWork;

        public TableController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [ProducesResponseType(typeof(IEnumerable<DaTable>),StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _unitOfWork.TableRepository.GetAllAsync());
        }

        [ProducesResponseType(typeof(IEnumerable<DaTable>), StatusCodes.Status200OK)]
        [HttpPost("PersonAndTableDTO")]
        public async Task<IActionResult> GetTablesBookedByPerson()
        {
            return Ok(await _unitOfWork.BookingRepository.PersonTableSummaryAsync());
        }
    }
}
