using System;
using System.Collections.Generic;
using AutoMapper;
using Library.API.Entities;
using Library.API.Helpers;
using Library.API.Models;
using Library.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [Route("api/authors/{authorId}/books")]
    public class BooksController : Controller
    {
        private readonly ILibraryRepository libraryRepository;

        public BooksController(ILibraryRepository libraryRepository)
        {
            this.libraryRepository = libraryRepository;
        }

        [HttpGet]
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
                return this.Ok(booksForAuthor);
            }
        }

        [HttpGet("{id}", Name = nameof(GetBookForAuthor))]
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
                    return this.Ok(bookForAuthor);
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

                    return this.CreatedAtRoute(nameof(GetBookForAuthor), new { authorId, id = bookToReturn.Id }, bookToReturn);
                }
            }
        }

        [HttpDelete("{id}")]
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
                        return this.NoContent();
                    }
                }
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBookForAuthor(Guid authorId, Guid id, [FromBody]BookForUpdateDto book)
        {
            if (book == null)
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
                    return this.NotFound();
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
                        return this.Ok(bookToReturn);
                    }
                }
            }
        }
    }
}