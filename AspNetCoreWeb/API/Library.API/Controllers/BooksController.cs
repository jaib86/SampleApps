using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Library.API.Entities;
using Library.API.Helpers;
using Library.API.Models;
using Library.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Library.API.Controllers
{
    [Route("api/authors/{authorId}/books")]
    public class BooksController : Controller
    {
        private readonly ILibraryRepository libraryRepository;
        private readonly ILogger<BooksController> logger;
        private readonly IUrlHelper urlHelper;

        public BooksController(ILibraryRepository libraryRepository, ILogger<BooksController> logger, IUrlHelper urlHelper)
        {
            this.libraryRepository = libraryRepository;
            this.logger = logger;
            this.urlHelper = urlHelper;
        }

        [HttpGet(Name = "GetBooksForAuthor")]
        public IActionResult GetBooksForAuthor(Guid authorId)
        {
            if (!this.libraryRepository.AuthorExists(authorId))
            {
                return this.NotFound();
            }
            else
            {
                var booksForAuthorFromRepo = this.libraryRepository.GetBooksForAuthor(authorId);

                var booksForAuthor = Mapper.Map<IEnumerable<BookDto>>(booksForAuthorFromRepo);

                booksForAuthor = booksForAuthor.Select(book => this.CreateLinksForBook(book));

                var wrapper = new LinkedCollectionResourceWrapperDto<BookDto>(booksForAuthor);

                return this.Ok(this.CreateLinksForBooks(wrapper));
            }
        }

        [HttpGet("{id}", Name = "GetBookForAuthor")]
        public IActionResult GetBookForAuthor(Guid authorId, Guid id)
        {
            if (!this.libraryRepository.AuthorExists(authorId))
            {
                return this.NotFound();
            }
            else
            {
                var bookForAuthorFromRepo = this.libraryRepository.GetBookForAuthor(authorId, id);

                if (bookForAuthorFromRepo == null)
                {
                    return this.NotFound();
                }
                else
                {
                    var bookForAuthor = Mapper.Map<BookDto>(bookForAuthorFromRepo);
                    return this.Ok(this.CreateLinksForBook(bookForAuthor));
                }
            }
        }

        [HttpPost]
        public IActionResult CreateBookForAuthor(Guid authorId, [FromBody]BookForCreationDto book)
        {
            if (book == null)
            {
                return this.BadRequest();
            }

            if (book.Title == book.Description)
            {
                this.ModelState.AddModelError(nameof(BookForCreationDto), "The provided description should be different from the title.");
            }

            if (!this.ModelState.IsValid)
            {
                // return 422 - Unprocessable Entity
                return new UnprocessableEntityObjectResult(this.ModelState);
            }
            else if (!this.libraryRepository.AuthorExists(authorId))
            {
                return this.NotFound();
            }
            else
            {
                var bookEntity = Mapper.Map<Book>(book);

                this.libraryRepository.AddBookForAuthor(authorId, bookEntity);

                if (!this.libraryRepository.Save())
                {
                    throw new LibraryException($"Creating a book for author {authorId} failed on save.");
                }
                else
                {
                    var bookToReturn = Mapper.Map<BookDto>(bookEntity);

                    return this.CreatedAtRoute(nameof(GetBookForAuthor), new { authorId, id = bookToReturn.Id }, this.CreateLinksForBook(bookToReturn));
                }
            }
        }

        [HttpDelete("{id}", Name = "DeleteBookForAuthor")]
        public IActionResult DeleteBookForAuthor(Guid authorId, Guid id)
        {
            if (!this.libraryRepository.AuthorExists(authorId))
            {
                return this.NotFound();
            }
            else
            {
                var bookForAuthorFromRepo = this.libraryRepository.GetBookForAuthor(authorId, id);
                if (bookForAuthorFromRepo == null)
                {
                    return this.NotFound();
                }
                else
                {
                    this.libraryRepository.DeleteBook(bookForAuthorFromRepo);

                    if (!this.libraryRepository.Save())
                    {
                        throw new LibraryException($"Deleting book {id} for author {authorId} failed on save.");
                    }
                    else
                    {
                        this.logger.LogInformation(100, $"Book {id} for author {authorId} was deleted.");

                        return this.NoContent();
                    }
                }
            }
        }

        [HttpPut("{id}", Name = "UpdateBookForAuthor")]
        public IActionResult UpdateBookForAuthor(Guid authorId, Guid id, [FromBody]BookForUpdateDto book)
        {
            if (book == null)
            {
                return this.BadRequest();
            }

            if (book.Title == book.Description)
            {
                this.ModelState.AddModelError(nameof(BookForUpdateDto), "The provided description should be different from the title.");
            }

            if (!this.ModelState.IsValid)
            {
                // return 422 - Unprocessable Entity
                return new UnprocessableEntityObjectResult(this.ModelState);
            }
            else if (!this.libraryRepository.AuthorExists(authorId))
            {
                return this.NotFound();
            }
            else
            {
                var bookForAuthorFromRepo = this.libraryRepository.GetBookForAuthor(authorId, id);

                if (bookForAuthorFromRepo == null)
                {
                    var bookToAdd = Mapper.Map<Book>(book);
                    bookToAdd.Id = id;

                    this.libraryRepository.AddBookForAuthor(authorId, bookToAdd);

                    if (!this.libraryRepository.Save())
                    {
                        throw new LibraryException($"Upserting book {id} for author {authorId} failed on save.");
                    }
                    else
                    {
                        var bookToReturn = Mapper.Map<BookDto>(bookToAdd);
                        return this.CreatedAtRoute(nameof(GetBookForAuthor), new { authorId, id = bookToReturn.Id }, bookToReturn);
                    }
                }
                else
                {
                    Mapper.Map(book, bookForAuthorFromRepo);
                    this.libraryRepository.UpdateBookForAuthor(bookForAuthorFromRepo);

                    if (!this.libraryRepository.Save())
                    {
                        throw new LibraryException($"Updating book {id} for author {authorId} failed on save.");
                    }
                    else
                    {
                        var bookToReturn = Mapper.Map<BookDto>(bookForAuthorFromRepo);

                        // Its up to us to either return 204-No Content or 200-OK status
                        return this.Ok(bookToReturn);
                    }
                }
            }
        }

        [HttpPatch("{id}", Name = "PartiallyUpdateBookForAuthor")]
        public IActionResult PartiallyUpdateBookForAuthor(Guid authorId, Guid id, [FromBody]JsonPatchDocument<BookForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return this.BadRequest();
            }
            else if (!this.libraryRepository.AuthorExists(authorId))
            {
                return this.NotFound();
            }
            else
            {
                var bookForAuthorFromRepo = this.libraryRepository.GetBookForAuthor(authorId, id);

                if (bookForAuthorFromRepo == null)
                {
                    // Return 404 when resource not found.
                    // return this.NotFound()

                    // Upserting
                    var bookDto = new BookForUpdateDto();
                    patchDoc.ApplyTo(bookDto);

                    // add validation
                    if (bookDto.Title == bookDto.Description)
                    {
                        this.ModelState.AddModelError(nameof(BookForUpdateDto), "The provided description should be different from the title.");
                    }

                    this.TryValidateModel(bookDto);

                    if (!this.ModelState.IsValid)
                    {
                        return new UnprocessableEntityObjectResult(this.ModelState);
                    }

                    var bookToAdd = Mapper.Map<Book>(bookDto);
                    bookToAdd.Id = id;
                    this.libraryRepository.AddBookForAuthor(authorId, bookToAdd);
                    if (!this.libraryRepository.Save())
                    {
                        throw new LibraryException($"Upserting book {id} for author {authorId} failed on save.");
                    }
                    else
                    {
                        var bookToReturn = Mapper.Map<BookDto>(bookToAdd);
                        return this.CreatedAtRoute(nameof(GetBookForAuthor), new { authorId = bookToReturn.AuthorId, id = bookToReturn.Id }, bookToReturn);
                    }
                }
                else
                {
                    var bookToPatch = Mapper.Map<BookForUpdateDto>(bookForAuthorFromRepo);

                    patchDoc.ApplyTo(bookToPatch, this.ModelState);

                    // add validation
                    if (bookToPatch.Title == bookToPatch.Description)
                    {
                        this.ModelState.AddModelError(nameof(BookForUpdateDto), "The provided description should be different from the title.");
                    }

                    this.TryValidateModel(bookToPatch);

                    if (!this.ModelState.IsValid)
                    {
                        return new UnprocessableEntityObjectResult(this.ModelState);
                    }

                    Mapper.Map(bookToPatch, bookForAuthorFromRepo);
                    this.libraryRepository.UpdateBookForAuthor(bookForAuthorFromRepo);

                    if (!this.libraryRepository.Save())
                    {
                        throw new LibraryException($"Patching book {id} for author {authorId} failed on save.");
                    }
                    else
                    {
                        return this.NoContent();
                    }
                }
            }
        }

        private BookDto CreateLinksForBook(BookDto book)
        {
            book.Links.Add(new LinkDto(this.urlHelper.Link("GetBookForAuthor", new { id = book.Id }), "self", "GET"));
            book.Links.Add(new LinkDto(this.urlHelper.Link("DeleteBookForAuthor", new { id = book.Id }), "delete_book", "DELETE"));
            book.Links.Add(new LinkDto(this.urlHelper.Link("UpdateBookForAuthor", new { id = book.Id }), "update_book", "PUT"));
            book.Links.Add(new LinkDto(this.urlHelper.Link("UpdateBookForAuthor", new { id = book.Id }), "partially_update_book", "PATCH"));

            return book;
        }

        private LinkedCollectionResourceWrapperDto<BookDto> CreateLinksForBooks(LinkedCollectionResourceWrapperDto<BookDto> booksWrapper)
        {
            booksWrapper.Links.Add(new LinkDto(this.urlHelper.Link("GetBooksForAuthor", new { }), "self", "GET"));

            return booksWrapper;
        }
    }
}