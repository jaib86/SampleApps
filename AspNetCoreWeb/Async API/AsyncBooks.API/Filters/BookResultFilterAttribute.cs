using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace AsyncBooks.API.Filters
{
    public class BookResultFilterAttribute : ResultFilterAttribute
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

            if (actionResult.Value is Entities.Book)
            {
                actionResult.Value = mapper.Map<Models.Book>(actionResult.Value);
            }
            else if (actionResult.Value is IEnumerable<Entities.Book>)
            {
                actionResult.Value = mapper.Map<IEnumerable<Models.Book>>(actionResult.Value);
            }

            await next();
        }
    }
}