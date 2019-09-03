using System;
using System.Collections.Generic;
using AutoMapper;
using Library.API.Models;
using Library.API.Services;
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
            try
            {
                throw new Exception("Random exception for testing purpose");

                var authorsFromRepo = this.libraryRepository.GetAuthors();
                var authors = Mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo);
                return this.Ok(authors);
            }
            catch (Exception)
            {
                return this.StatusCode(500, "An unexpected fault happened. Try again later.");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetAuthor(Guid id)
        {
            var authorFromRepo = this.libraryRepository.GetAuthor(id);

            if (authorFromRepo == null)
            {
                throw new Exception("Random exception for testing purpose");
                return this.NotFound();
            }
            else
            {
                var author = Mapper.Map<AuthorDto>(authorFromRepo);
                return this.Ok(author);
            }
        }
    }
}