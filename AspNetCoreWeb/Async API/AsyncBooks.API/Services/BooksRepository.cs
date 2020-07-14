using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AsyncBooks.API.Contexts;
using AsyncBooks.API.Entities;
using AsyncBooks.API.ExternalModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AsyncBooks.API.Services
{
    public class BooksRepository : IBooksRepository, IDisposable
    {
        private BooksContext context;
        private readonly ILogger<BooksRepository> logger;
        private readonly IHttpClientFactory httpClientFactory;
        private CancellationTokenSource cancellationTokenSource;

        public BooksRepository(BooksContext context, ILogger<BooksRepository> logger, IHttpClientFactory httpClientFactory)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<Book> GetBookAsync(Guid id)
        {
            return await this.context.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            ///this.logger.LogInformation(System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
            ///await this.context.Database.ExecuteSqlCommandAsync("WAITFOR DELAY '00:00:02'");
            ///this.logger.LogInformation(System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
            var result = await this.context.Books.Include(b => b.Author).ToListAsync();
            ///this.logger.LogInformation(System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
            return result;
        }

        public async Task<IEnumerable<Book>> GetBooksAsync(IEnumerable<Guid> bookIds)
        {
            return await this.context.Books
                                     .Where(b => bookIds.Contains(b.Id))
                                     .Include(b => b.Author).ToListAsync();
        }

        public async Task<BookCover> GetBookCoverAsync(string coverId)
        {
            var httpClient = this.httpClientFactory.CreateClient();

            // pass through a dummy name
            var response = await httpClient.GetAsync($"https://localhost:44389/api/bookcovers/{coverId}");
            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<BookCover>(await response.Content.ReadAsStringAsync(),
                                                                new JsonSerializerOptions
                                                                {
                                                                    PropertyNameCaseInsensitive = true
                                                                });
            }

            return null;
        }

        public async Task<IEnumerable<BookCover>> GetBookCoversAsync(Guid bookId)
        {
            var httpClient = this.httpClientFactory.CreateClient();
            this.cancellationTokenSource = new CancellationTokenSource();

            // create a list of fake bookcovers
            var bookCoverUrls = new[]
            {
                $"https://localhost:44389/api/bookcovers/{bookId}-dummycover1",
                //$"https://localhost:44389/api/bookcovers/{bookId}-dummycover2?returnFault=true",
                $"https://localhost:44389/api/bookcovers/{bookId}-dummycover2",
                $"https://localhost:44389/api/bookcovers/{bookId}-dummycover3",
                $"https://localhost:44389/api/bookcovers/{bookId}-dummycover4",
                $"https://localhost:44389/api/bookcovers/{bookId}-dummycover5"
            };

            // create the tasks
            var downloadBookCoverTaskQuery = from bookCoverUrl in bookCoverUrls
                                             select DownloadBookCoverAsync(httpClient, bookCoverUrl, this.cancellationTokenSource.Token);

            // start the tasks
            var downloadBookCoverTasks = downloadBookCoverTaskQuery.ToList();

            try
            {
                return await Task.WhenAll(downloadBookCoverTasks);
            }
            catch (OperationCanceledException ex)
            {
                this.logger.LogInformation($"Error: {ex.Message}");
                foreach (var task in downloadBookCoverTasks)
                {
                    this.logger.LogInformation($"Task {task.Id} has status {task.Status}");
                }

                return new List<BookCover>();
            }
        }

        private async Task<BookCover> DownloadBookCoverAsync(HttpClient httpClient, string bookCoverUrl, CancellationToken cancellationToken)
        {
            var response = await httpClient.GetAsync(bookCoverUrl, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var bookCover = JsonSerializer.Deserialize<BookCover>(
                    await response.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                return bookCover;
            }

            this.cancellationTokenSource.Cancel();

            return null;
        }

        public void AddBook(Book bookToAdd)
        {
            if (bookToAdd == null)
            {
                throw new ArgumentNullException(nameof(bookToAdd));
            }

            this.context.Add(bookToAdd);
        }

        public IEnumerable<Book> GetBooks()
        {
            ///this.context.Database.ExecuteSqlCommand("WAITFOR DELAY '00:00:02'");
            return this.context.Books.Include(b => b.Author).ToList();
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
            if (disposing)
            {
                if (this.context != null)
                {
                    this.context.Dispose();
                    this.context = null;
                }

                if (this.cancellationTokenSource != null)
                {
                    this.cancellationTokenSource.Dispose();
                    this.cancellationTokenSource = null;
                }
            }
        }
    }
}