﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CacheCow.Server.Core.Mvc;
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
        [HttpHead]
        public IActionResult GetAuthors(AuthorsResourceParameters authorsResourceParameters, [FromHeader(Name = "Accept")] string mediaType)
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

            var authors = Mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo);

            if (mediaType == "application/vnd.nagarro.hateoas+json")
            {
                var links = this.CreateLinksForAuthors(authorsResourceParameters, authorsFromRepo.HasNext, authorsFromRepo.HasPrevious);

                var paginationMetadata = new
                {
                    totalCount = authorsFromRepo.TotalCount,
                    pageSize = authorsFromRepo.PageSize,
                    currentPage = authorsFromRepo.CurrentPage,
                    totalPages = authorsFromRepo.TotalPages
                };

                this.Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));

                var shapedAuthors = authors.ShapeData(authorsResourceParameters.Fields);

                var shapedAuthorsWithLinks = shapedAuthors.Select(author =>
                {
                    var authorAsDictionary = author as IDictionary<string, object>;
                    var authorLinks = this.CreateLinksForAuthor((Guid)authorAsDictionary["Id"], authorsResourceParameters.Fields);
                    authorAsDictionary.Add("links", authorLinks);
                    return authorAsDictionary;
                });

                var linkedCollectionResource = new { value = shapedAuthorsWithLinks, links };

                return this.Ok(linkedCollectionResource);
            }
            else
            {
                var previousPageLink = authorsFromRepo.HasPrevious ? this.CreateAuthorsResourceUri(authorsResourceParameters, ResourceUriType.PreviousPage) : null;
                var nextPageLink = authorsFromRepo.HasNext ? this.CreateAuthorsResourceUri(authorsResourceParameters, ResourceUriType.NextPage) : null;

                var paginationMetadata = new
                {
                    previousPageLink,
                    nextPageLink,
                    totalCount = authorsFromRepo.TotalCount,
                    pageSize = authorsFromRepo.PageSize,
                    currentPage = authorsFromRepo.CurrentPage,
                    totalPages = authorsFromRepo.TotalPages
                };

                this.Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));

                return this.Ok(authors.ShapeData(authorsResourceParameters.Fields));
            }
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

                case ResourceUriType.Current:
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
        [HttpCacheFactory(60)]
        public IActionResult GetAuthor(Guid id, [FromQuery] string fields, [FromHeader(Name = "Accept")] string mediaType)
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

                if (mediaType == "application/vnd.nagarro.hateoas+json")
                {
                    var linkedResourceToReturn = author.ShapeData(fields) as IDictionary<string, object>;
                    linkedResourceToReturn.Add("links", this.CreateLinksForAuthor(id, fields));

                    return this.Ok(linkedResourceToReturn);
                }
                else
                {
                    return this.Ok(author.ShapeData(fields));
                }
            }
        }

        [HttpPost(Name = "CreateAuthor")]
        [RequestHeaderMatchesMediaType("Content-Type", new[] { "application/vnd.nagarro.author.full+json" })]
        public IActionResult CreateAuthor([FromBody]AuthorForCreationDto author, [FromHeader(Name = "Accept")] string mediaType)
        {
            if (author == null)
            {
                return this.BadRequest();
            }

            var authorEntity = Mapper.Map<Author>(author);

            this.libraryRepository.AddAuthor(authorEntity);

            if (!this.libraryRepository.Save())
            {
                throw new LibraryException("Creating an author failed on save.");
            }
            else
            {
                var authorToReturn = Mapper.Map<AuthorDto>(authorEntity);

                if (mediaType == "application/vnd.nagarro.hateoas+json")
                {
                    var links = this.CreateLinksForAuthor(authorToReturn.Id, null);

                    var linkedResourceToReturn = authorToReturn.ShapeData(null) as IDictionary<string, object>;
                    linkedResourceToReturn.Add("links", links);

                    return this.CreatedAtRoute("GetAuthor", new { id = linkedResourceToReturn["Id"] }, linkedResourceToReturn);
                }
                else
                {
                    var resourceToReturn = authorToReturn.ShapeData(null) as IDictionary<string, object>;
                    return this.CreatedAtRoute("GetAuthor", new { id = resourceToReturn["Id"] }, resourceToReturn);
                }
            }
        }

        [HttpPost(Name = "CreateAuthorWithDateOfDeath")]
        [RequestHeaderMatchesMediaType("Content-Type",
            new[] { "application/vnd.nagarro.authorwithdateofdeath.full+json", "application/vnd.nagarro.authorwithdateofdeath.full+xml" })]
        [RequestHeaderMatchesMediaType("Accept", new[] { "application/vnd.nagarro.hateoas+json", "application/vnd.nagarro.hateoas+xml" })]
        public IActionResult CreateAuthorWithDateOfDeath([FromBody]AuthorForCreationWithDateOfDeatchDto author, [FromHeader(Name = "Accept")]string mediaType)
        {
            if (author == null)
            {
                return this.BadRequest();
            }

            var authorEntity = Mapper.Map<Author>(author);

            this.libraryRepository.AddAuthor(authorEntity);

            if (!this.libraryRepository.Save())
            {
                throw new LibraryException("Creating an author failed on save.");
            }
            else
            {
                var authorToReturn = Mapper.Map<AuthorDto>(authorEntity);

                if (mediaType == "application/vnd.nagarro.hateoas+json")
                {
                    var links = this.CreateLinksForAuthor(authorToReturn.Id, null);

                    var linkedResourceToReturn = authorToReturn.ShapeData(null) as IDictionary<string, object>;
                    linkedResourceToReturn.Add("links", links);

                    return this.CreatedAtRoute("GetAuthor", new { id = linkedResourceToReturn["Id"] }, linkedResourceToReturn);
                }
                else
                {
                    return this.CreatedAtRoute("GetAuthor", new { id = authorToReturn.Id }, authorToReturn);
                }
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

        [HttpDelete("{id}", Name = "DeleteAuthor")]
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

        [HttpOptions]
        public IActionResult GetAuthorsOptions()
        {
            this.Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return this.Ok();
        }

        private IEnumerable<LinkDto> CreateLinksForAuthor(Guid id, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(new LinkDto(this.urlHelper.Link("GetAuthor", new { id }), "self", "GET"));
            }
            else
            {
                links.Add(new LinkDto(this.urlHelper.Link("GetAuthor", new { id, fields }), "self", "GET"));
            }

            links.Add(new LinkDto(this.urlHelper.Link("GetBooksForAuthor", new { authorId = id }), "books", "GET"));
            links.Add(new LinkDto(this.urlHelper.Link("CreateBookForAuthor", new { authorId = id }), "create_book_for_author", "POST"));
            links.Add(new LinkDto(this.urlHelper.Link("DeleteAuthor", new { id }), "delete_author", "DELETE"));

            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForAuthors(AuthorsResourceParameters authorsResourceParameters, bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>
            {
                // self
                new LinkDto(this.CreateAuthorsResourceUri(authorsResourceParameters, ResourceUriType.Current), "self", "GET")
            };

            if (hasNext)
            {
                links.Add(new LinkDto(this.CreateAuthorsResourceUri(authorsResourceParameters, ResourceUriType.NextPage), "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(new LinkDto(this.CreateAuthorsResourceUri(authorsResourceParameters, ResourceUriType.PreviousPage),
                          "previousPage", "GET"));
            }

            return links;
        }
    }
}