﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Books.Api.Filters
{
    public class BooksResultFilterAttribute : ResultFilterAttribute
    {
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var resultFromAction = context.Result as ObjectResult;

            if (resultFromAction.Value == null
                || resultFromAction.StatusCode < 200
                || resultFromAction.StatusCode >= 300)
            {
                await next();
                return;
            }

            if (context.HttpContext.RequestServices.GetService(typeof(IMapper)) is IMapper mapper)
            {
                resultFromAction.Value = mapper.Map<IEnumerable<Models.Book>>(resultFromAction.Value);
            }

            await next();
        }
    }
}