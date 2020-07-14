using System;
using AsyncBooks.API.Filters;
using AsyncBooks.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AsyncBooks.API.Controllers
{
    [ApiController]
    [Route("api/synchronous-books")]
    public class SynchronousBooksController : ControllerBase
    {
        private readonly IBooksRepository booksRepository;

        public SynchronousBooksController(IBooksRepository booksRepository)
        {
            this.booksRepository = booksRepository ?? throw new ArgumentNullException(nameof(booksRepository));
        }

        [HttpGet]
        [BookResultFilter]
        public IActionResult GetBooks()
        {
            var bookEntities = booksRepository.GetBooks();
            return Ok(bookEntities);
        }
    }
}