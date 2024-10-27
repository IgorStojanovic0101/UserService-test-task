using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Test.Application.Abstraction;
using Test.Application.Services;

namespace Test.WebAPI.Utilities.middlewares
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        private IErrorService _errorService;

        public GlobalExceptionHandlingMiddleware(IErrorService errorService) =>  _errorService = errorService;
        

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _errorService.AddErrorRecord(ex.Message, ex);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                context.Response.ContentType = "application/json";
                var response = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "An unexpected error occurred. Please try again later.",
                    Detail = ex.Message 
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
