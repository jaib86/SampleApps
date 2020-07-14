using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AsyncBooks.API.Filters;
using AsyncBooks.API.ModelBinders;
using AsyncBooks.API.Models;
using AsyncBooks.API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AsyncBooks.API.Controllers
{
    [ApiController]
    [Route("api/bookcollections")]
    [BookResultFilter]
    public class BookCollectionsController : ControllerBase
    {
        private readonly IBooksRepository booksRepository;

        public BookCollectionsController(IBooksRepository booksRepository)
        {
            this.booksRepository = booksRepository ?? throw new ArgumentNullException(nameof(booksRepository));
        }

        // api/bookcollections/(id1,id2,...)
        [HttpGet("({bookIds})", Name = nameof(GetBookCollection))]
        public async Task<IActionResult> GetBookCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> bookIds)
        {
            var bookEntities = await this.booksRepository.GetBooksAsync(bookIds);

            if (bookIds.Count() != bookEntities.Count())
            {
                return NotFound();
            }

            return Ok(bookEntities);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBookCollection(IEnumerable<BookForCreation> bookCollection, [FromServices] IMapper mapper)
        {
            var bookEntities = mapper.Map<IEnumerable<Entities.Book>>(bookCollection);

            foreach (var bookEntity in bookEntities)
            {
                booksRepository.AddBook(bookEntity);
            }

            await booksRepository.SaveChangesAsync();

            var bookIds = bookEntities.Select(x => x.Id);
            var booksToReturn = await this.booksRepository.GetBooksAsync(bookIds);

            return CreatedAtRoute(nameof(GetBookCollection), new { bookIds = string.Join(",", bookIds) }, booksToReturn);
        }
    }
}