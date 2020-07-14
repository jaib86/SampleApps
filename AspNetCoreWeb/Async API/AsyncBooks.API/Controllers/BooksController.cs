using System;
using System.Threading.Tasks;
using AsyncBooks.API.Filters;
using AsyncBooks.API.Models;
using AsyncBooks.API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AsyncBooks.API.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly IBooksRepository booksRepository;
        private readonly ILogger<BooksController> logger;

        public BooksController(IBooksRepository booksRepository, ILogger<BooksController> logger)
        {
            this.booksRepository = booksRepository ?? throw new ArgumentNullException(nameof(booksRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [BookResultFilter]
        public async Task<IActionResult> GetBooks()
        {
            this.logger.LogInformation(System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
            var bookEntities = await this.booksRepository.GetBooksAsync();
            return Ok(bookEntities);
        }

        [HttpGet("{id}", Name = nameof(GetBook))]
        [BookWithCoversResultFilter]
        public async Task<IActionResult> GetBook(Guid id)
        {
            var bookEntity = await this.booksRepository.GetBookAsync(id);

            if (bookEntity == null)
            {
                return NotFound();
            }

            var bookCovers = await this.booksRepository.GetBookCoversAsync(id);

            // Tuple
            return Ok((bookEntity, bookCovers));
        }

        [HttpPost]
        [BookResultFilter]
        public async Task<IActionResult> CreateBook(BookForCreation bookForCreation, [FromServices] IMapper mapper)
        {
            var bookEntity = mapper.Map<Entities.Book>(bookForCreation);

            this.booksRepository.AddBook(bookEntity);

            await this.booksRepository.SaveChangesAsync();

            // Fetch (refresh) the book from the data store, including the author
            await this.booksRepository.GetBookAsync(bookEntity.Id);

            return CreatedAtRoute(nameof(GetBook), new { id = bookEntity.Id }, bookEntity);
        }
    }
}