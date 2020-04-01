using System;
using System.Threading.Tasks;
using AutoMapper;
using Books.Api.Filters;
using Books.Api.Models;
using Books.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Books.Api.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBooksRepository booksRepository;
        private readonly IMapper mapper;

        public BooksController(IBooksRepository booksRepository, IMapper mapper)
        {
            this.booksRepository = booksRepository ?? throw new ArgumentNullException(nameof(booksRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [BooksResultFilter]
        public async Task<IActionResult> GetBooks()
        {
            var bookEntities = await this.booksRepository.GetBooksAsync();
            return this.Ok(bookEntities);
        }

        [HttpGet]
        [BookResultFilter]
        [Route("{id}", Name = "GetBook")]
        public async Task<IActionResult> GetBook(Guid id)
        {
            var bookEntity = await this.booksRepository.GetBookAsync(id);

            if (bookEntity == null)
            {
                return this.NotFound();
            }
            else
            {
                return this.Ok(bookEntity);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] BookForCreation book)
        {
            var bookEntity = this.mapper.Map<Entities.Book>(book);
            this.booksRepository.AddBook(bookEntity);

            await this.booksRepository.SaveChangesAsync();

            return this.CreatedAtRoute("GetBook", new { id = bookEntity.Id }, bookEntity);
        }
    }
}