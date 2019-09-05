using System;
using System.Collections.Generic;
using AutoMapper;
using Library.API.Entities;
using Library.API.Helpers;
using Library.API.Models;
using Library.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [Route("api/authors")]
    public class AuthorsController : Controller
    {
        private readonly ILibraryRepository libraryRepository;

        public AuthorsController(ILibraryRepository libraryRepository)
        {
            this.libraryRepository = libraryRepository;
        }

        [HttpGet]
        public IActionResult GetAuthors()
        {
            var authorsFromRepo = this.libraryRepository.GetAuthors();
            var authors = Mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo);
            return this.Ok(authors);
        }

        [HttpGet("{id}", Name = nameof(GetAuthor))]
        public IActionResult GetAuthor(Guid id)
        {
            var authorFromRepo = this.libraryRepository.GetAuthor(id);

            if (authorFromRepo == null)
            {
                return this.NotFound();
            }
            else
            {
                var author = Mapper.Map<AuthorDto>(authorFromRepo);
                return this.Ok(author);
            }
        }

        [HttpPost]
        public IActionResult CreateAuthor([FromBody]AuthorForCreationDto author)
        {
            var authorEntity = Mapper.Map<Author>(author);

            this.libraryRepository.AddAuthor(authorEntity);

            if (!this.libraryRepository.Save())
            {
                throw new LibraryException("Creating an author failed on save.");
            }
            else
            {
                var authorToReturn = Mapper.Map<AuthorDto>(authorEntity);

                return this.CreatedAtRoute(nameof(GetAuthor), new { id = authorToReturn.Id }, authorToReturn);
            }
        }

        [HttpPost("{id}")]
        public IActionResult BlockAuthorCreation(Guid id)
        {
            if (this.libraryRepository.AuthorExists(id))
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }
            else
            {
                return this.NotFound();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAuthor(Guid id)
        {
            var authorFromRepo = this.libraryRepository.GetAuthor(id);

            if (authorFromRepo == null)
            {
                return this.NotFound();
            }
            else
            {
                this.libraryRepository.DeleteAuthor(authorFromRepo);

                if (!this.libraryRepository.Save())
                {
                    throw new LibraryException($"Delete author {id} failed on save.");
                }
                else
                {
                    return this.NoContent();
                }
            }
        }
    }
}