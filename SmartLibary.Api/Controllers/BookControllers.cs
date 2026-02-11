using Mapster;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.Application.DTOs;
using SmartLibrary.Application.Interfaces;
using SmartLibrary.Domain;

namespace SmartLibrary.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepo;

        public BooksController(IBookRepository bookRepo)
        {
            _bookRepo = bookRepo;
        }

        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] CreateBookRequest request)
        {
            // Мапване от Request към Domain модел
            var book = request.Adapt<Book>();
            book.Id = Guid.NewGuid();

            await _bookRepo.CreateAsync(book);

            return CreatedAtAction(nameof(AddBook), new { id = book.Id }, book);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var books = await _bookRepo.GetAllAsync();
            return Ok(books);
        }
    }
}
