using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncBooks.API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace AsyncBooks.API.Filters
{
    public class BookWithCoversResultFilterAttribute : ResultFilterAttribute
    {
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var actionResult = context.Result as ObjectResult;

            if (actionResult.Value == null || actionResult.StatusCode < 200 || actionResult.StatusCode >= 300)
            {
                await next();
                return;
            }

            var mapper = context.HttpContext.RequestServices.GetRequiredService<IMapper>();

            var (book, bookCovers) = ((Entities.Book, IEnumerable<ExternalModels.BookCover>))actionResult.Value;

            var mappedBook = mapper.Map<BookWithCovers>(book);
            actionResult.Value = mapper.Map(bookCovers, mappedBook);

            await next();
        }
    }
}