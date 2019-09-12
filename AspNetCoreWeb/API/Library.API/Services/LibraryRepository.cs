using System;
using System.Collections.Generic;
using System.Linq;
using Library.API.Entities;
using Library.API.Helpers;
using Library.API.Models;

namespace Library.API.Services
{
    public class LibraryRepository : ILibraryRepository
    {
        private readonly LibraryContext context;
        private readonly IPropertyMappingService propertyMappingService;

        public LibraryRepository(LibraryContext context, IPropertyMappingService propertyMappingService)
        {
            this.context = context;
            this.propertyMappingService = propertyMappingService;
        }

        public void AddAuthor(Author author)
        {
            author.Id = Guid.NewGuid();
            this.context.Authors.Add(author);

            // the repository fills the id (instead of using identity columns)
            if (author.Books.Count > 0)
            {
                foreach (var book in author.Books)
                {
                    book.Id = Guid.NewGuid();
                }
            }
        }

        public void AddBookForAuthor(Guid authorId, Book book)
        {
            var author = this.GetAuthor(authorId);

            if (author != null)
            {
                // if there isn't an id filled out, we should generate one
                if (book.Id == default)
                {
                    book.Id = Guid.NewGuid();
                }
                author.Books.Add(book);
            }
        }

        public bool AuthorExists(Guid authorId)
        {
            return this.context.Authors.Any(a => a.Id == authorId);
        }

        public void DeleteAuthor(Author author)
        {
            this.context.Authors.Remove(author);
        }

        public void DeleteBook(Book book)
        {
            this.context.Books.Remove(book);
        }

        public Author GetAuthor(Guid authorId)
        {
            return this.context.Authors.FirstOrDefault(a => a.Id == authorId);
        }

        public PagedList<Author> GetAuthors(AuthorsResourceParameters authorsResourceParameters)
        {
            //var collectionBeforePaging = this.context.Authors.OrderBy(a => a.FirstName).ThenBy(a => a.LastName).AsQueryable()

            var collectionBeforePaging = this.context.Authors.ApplySort(authorsResourceParameters.OrderBy, this.propertyMappingService.GetPropertyMapping<AuthorDto, Author>());


            if (!string.IsNullOrEmpty(authorsResourceParameters.Genre))
            {
                // trim & ignore casing
                var genreForWhereClause = authorsResourceParameters.Genre.Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging.Where(a => a.Genre.ToLowerInvariant() == genreForWhereClause);
            }

            if (!string.IsNullOrEmpty(authorsResourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = authorsResourceParameters.SearchQuery.Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging.Where(a => a.Genre.ToLowerInvariant().Contains(searchQueryForWhereClause) || a.FirstName.ToLowerInvariant().Contains(searchQueryForWhereClause) || a.LastName.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            return PagedList<Author>.Create(collectionBeforePaging,
                                            authorsResourceParameters.PageNumber,
                                            authorsResourceParameters.PageSize);
        }

        public IEnumerable<Author> GetAuthors(IEnumerable<Guid> authorIDs)
        {
            return this.context.Authors.Where(a => authorIDs.Contains(a.Id)).OrderBy(a => a.FirstName).ThenBy(a => a.LastName);
        }

        public void UpdateAuthor(Author author)
        {
            // no code in this implementation
        }

        public Book GetBookForAuthor(Guid authorId, Guid bookId)
        {
            return this.context.Books.Where(b => b.AuthorId == authorId && b.Id == bookId).FirstOrDefault();
        }

        public IEnumerable<Book> GetBooksForAuthor(Guid authorId)
        {
            return this.context.Books.Where(b => b.AuthorId == authorId).OrderBy(b => b.Title).ToList();
        }

        public void UpdateBookForAuthor(Book book)
        {
            // no code in this implementation
        }

        public bool Save()
        {
            return this.context.SaveChanges() >= 0;
        }
    }
}