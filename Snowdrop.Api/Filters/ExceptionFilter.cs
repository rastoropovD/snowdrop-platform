using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Snowdrop.Infrastructure.Dto.Api;

namespace Snowdrop.Api.Filters
{
    public sealed class ExceptionFilter : IExceptionFilter
    {
        public async void OnException(ExceptionContext context)
        {
            context.ExceptionHandled = true;
            string message = context.Exception.Message;
            ErrorResponse response = new ErrorResponse(message);
            context.HttpContext.Response.Headers.Add(new KeyValuePair<string, StringValues>("Content-Type", "application/json"));
            await context
                  .HttpContext
                  .Response
                  .WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}