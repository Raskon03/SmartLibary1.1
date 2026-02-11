using Microsoft.AspNetCore.Mvc;
using SmartLibrary.Application.DTOs;
using SmartLibrary.Application.Interfaces;

namespace SmartLibrary.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LendingController : ControllerBase
    {
        private readonly ILendingService _lendingService;

        public LendingController(ILendingService lendingService)
        {
            _lendingService = lendingService;
        }

        [HttpPost("rent")]
        public async Task<IActionResult> RentBook([FromBody] RentBookRequest request)
        {
            try
            {
                var success = await _lendingService.RentBookAsync(request.UserId, request.BookId);

                if (!success)
                    return BadRequest("Книгата вече е заета или не е налична.");

                return Ok("Книгата е наета успешно.");
            }
            catch (Exception ex)
            {
                // Връщаме 404 ако книгата или потребителят ги няма
                return NotFound(ex.Message);
            }
        }
    }
}