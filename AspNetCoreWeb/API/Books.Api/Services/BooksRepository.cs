using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Books.Api.Contexts;
using Books.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Books.Api.Services
{
    public class BooksRepository : IBooksRepository, IDisposable
    {
        private BooksContext context;

        public BooksRepository(BooksContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Book> GetBookAsync(Guid id)
        {
            return await this.context.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            await this.context.Database.ExecuteSqlCommandAsync("WAITFOR DELAY '00:00:02';");
            return await this.context.Books.Include(b => b.Author).ToListAsync();
        }

        public IEnumerable<Book> GetBooks()
        {
            this.context.Database.ExecuteSqlCommand("WAITFOR DELAY '00:00:02';");
            return this.context.Books.Include(b => b.Author).ToList();
        }

        public void AddBook(Book bookToAdd)
        {
            if (bookToAdd == null)
            {
                throw new ArgumentNullException(nameof(bookToAdd));
            }

            this.context.Add(bookToAdd);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await this.context.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && this.context != null)
            {
                this.context.Dispose();
                this.context = null;
            }
        }
    }
}