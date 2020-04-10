using Lecture6.Exceptions;
using Lecture6.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lecture6.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }catch(Exception exc)
            {
                await HandleException(context, exc);
            }
        }

        private Task HandleException(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            if(ex is StudentCannotDefendException)
            {
                return context.Response.WriteAsync(new ErrorDetails
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Student cannot defend - this is often caused by...."
                }.ToString());
            }

            return context.Response.WriteAsync(new ErrorDetails
            {
                StatusCode=StatusCodes.Status500InternalServerError,
                Message= "Error something happened!"
            }.ToString());
        }
    }
}
