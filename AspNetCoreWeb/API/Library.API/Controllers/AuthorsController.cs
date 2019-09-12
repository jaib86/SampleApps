using System;
using System.Collections.Generic;
using AutoMapper;
using Library.API.Entities;
using Library.API.Helpers;
using Library.API.Models;
using Library.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Library.API.Controllers
{
    [Route("api/authors")]
    public class AuthorsController : Controller
    {
        private readonly ILibraryRepository libraryRepository;
        private readonly IUrlHelper urlHelper;
        private readonly IPropertyMappingService propertyMappingService;
        private readonly ITypeHelperService typeHelperService;

        public AuthorsController(ILibraryRepository libraryRepository, IUrlHelper urlHelper,
                                 IPropertyMappingService propertyMappingService,
                                 ITypeHelperService typeHelperService)
        {
            this.libraryRepository = libraryRepository;
            this.urlHelper = urlHelper;
            this.propertyMappingService = propertyMappingService;
            this.typeHelperService = typeHelperService;
        }

        [HttpGet(Name = "GetAuthors")]
        public IActionResult GetAuthors(AuthorsResourceParameters authorsResourceParameters)
        {
            if (!this.propertyMappingService.ValidMappingExistsFor<AuthorDto, Author>(authorsResourceParameters.OrderBy))
            {
                return this.BadRequest();
            }

            if (!this.typeHelperService.TypeHasProperties<AuthorDto>(authorsResourceParameters.Fields))
            {
                return this.BadRequest();
            }

            var authorsFromRepo = this.libraryRepository.GetAuthors(authorsResourceParameters);

            var previousPageLink = authorsFromRepo.HasPrevious ? this.CreateAuthorsResourceUri(authorsResourceParameters, ResourceUriType.PreviousPage) : null;

            var nextPageLink = authorsFromRepo.HasNext ? this.CreateAuthorsResourceUri(authorsResourceParameters, ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                totalCount = authorsFromRepo.TotalCount,
                pageSize = authorsFromRepo.PageSize,
                currentPage = authorsFromRepo.CurrentPage,
                totalPages = authorsFromRepo.TotalPages,
                previousPageLink,
                nextPageLink
            };

            this.Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));

            var authors = Mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo);
            return this.Ok(authors.ShapeData(authorsResourceParameters.Fields));
        }

        private string CreateAuthorsResourceUri(AuthorsResourceParameters authorsResourceParameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return this.urlHelper.Link("GetAuthors",
                                                new
                                                {
                                                    fields = authorsResourceParameters.Fields,
                                                    orderBy = authorsResourceParameters.OrderBy,
                                                    searchQuery = authorsResourceParameters.SearchQuery,
                                                    genre = authorsResourceParameters.Genre,
                                                    pageNumber = authorsResourceParameters.PageNumber - 1,
                                                    pageSize = authorsResourceParameters.PageSize
                                                });

                case ResourceUriType.NextPage:
                    return this.urlHelper.Link("GetAuthors",
                                                new
                                                {
                                                    fields = authorsResourceParameters.Fields,
                                                    orderBy = authorsResourceParameters.OrderBy,
                                                    searchQuery = authorsResourceParameters.SearchQuery,
                                                    genre = authorsResourceParameters.Genre,
                                                    pageNumber = authorsResourceParameters.PageNumber + 1,
                                                    pageSize = authorsResourceParameters.PageSize
                                                });

                default:
                    return this.urlHelper.Link("GetAuthors",
                                                new
                                                {
                                                    fields = authorsResourceParameters.Fields,
                                                    orderBy = authorsResourceParameters.OrderBy,
                                                    searchQuery = authorsResourceParameters.SearchQuery,
                                                    genre = authorsResourceParameters.Genre,
                                                    pageNumber = authorsResourceParameters.PageNumber,
                                                    pageSize = authorsResourceParameters.PageSize
                                                });
            }
        }

        [HttpGet("{id}", Name = "GetAuthor")]
        public IActionResult GetAuthor(Guid id, [FromQuery] string fields)
        {
            if (!this.typeHelperService.TypeHasProperties<AuthorDto>(fields))
            {
                return this.BadRequest();
            }

            var authorFromRepo = this.libraryRepository.GetAuthor(id);

            if (authorFromRepo == null)
            {
                return this.NotFound();
            }
            else
            {
                var author = Mapper.Map<AuthorDto>(authorFromRepo);
                return this.Ok(author.ShapeData(fields));
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