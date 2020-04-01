using System;
using Books.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Books.Api.Controllers
{
    [Route("api/synchronousbooks")]
    [ApiController]
    public class SynchronousBooksController : ControllerBase
    {
        private readonly IBooksRepository booksRepository;

        public SynchronousBooksController(IBooksRepository booksRepository)
        {
            this.booksRepository = booksRepository ?? throw new ArgumentNullException(nameof(booksRepository));
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            var bookEntities = this.booksRepository.GetBooks();
            return this.Ok(bookEntities);
        }
    }
}