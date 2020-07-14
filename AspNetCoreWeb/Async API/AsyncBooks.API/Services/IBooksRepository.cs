using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncBooks.API.Entities;
using AsyncBooks.API.ExternalModels;

namespace AsyncBooks.API.Services
{
    public interface IBooksRepository
    {
        IEnumerable<Book> GetBooks();

        Task<IEnumerable<Book>> GetBooksAsync();

        Task<IEnumerable<Book>> GetBooksAsync(IEnumerable<Guid> bookIds);

        Task<Book> GetBookAsync(Guid id);

        Task<BookCover> GetBookCoverAsync(string coverId);

        Task<IEnumerable<BookCover>> GetBookCoversAsync(Guid bookId);

        void AddBook(Book bookToAdd);

        Task<bool> SaveChangesAsync();
    }
}