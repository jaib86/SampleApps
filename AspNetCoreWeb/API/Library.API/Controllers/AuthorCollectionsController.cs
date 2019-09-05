using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Library.API.Entities;
using Library.API.Helpers;
using Library.API.Models;
using Library.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [Route("api/authorcollections")]
    public class AuthorCollectionsController : Controller
    {
        private readonly ILibraryRepository libraryRepository;

        public AuthorCollectionsController(ILibraryRepository libraryRepository)
        {
            this.libraryRepository = libraryRepository;
        }

        [HttpPost]
        public IActionResult CreateAuthorCollection([FromBody]IEnumerable<AuthorForCreationDto> authorCollection)
        {
            if (authorCollection == null)
            {
                return this.BadRequest();
            }
            else
            {
                var authorEntities = Mapper.Map<IEnumerable<Author>>(authorCollection);

                foreach (var author in authorEntities)
                {
                    this.libraryRepository.AddAuthor(author);
                }

                if (!this.libraryRepository.Save())
                {
                    throw new LibraryException("Creating an author collection failed on save.");
                }
                else
                {
                    var authorColloectionToReturn = Mapper.Map<IEnumerable<AuthorDto>>(authorEntities);
                    var idsAsString = string.Join(",", authorColloectionToReturn.Select(a => a.Id));

                    return this.CreatedAtRoute("GetAuthorCollection", new { ids = idsAsString }, authorColloectionToReturn);
                }
            }
        }

        // (key1, key2, ...)
        [HttpGet("({ids})", Name = "GetAuthorCollection")]
        public IActionResult GetAuthorCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))]IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                return this.BadRequest();
            }
            else
            {
                var authorEntities = this.libraryRepository.GetAuthors(ids);

                if (ids.Count() != authorEntities.Count())
                {
                    return this.NotFound();
                }
                else
                {
                    var authorsToReturn = Mapper.Map<IEnumerable<AuthorDto>>(authorEntities);
                    return this.Ok(authorsToReturn);
                }
            }
        }
    }
}