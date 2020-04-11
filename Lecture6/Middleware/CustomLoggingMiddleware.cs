using Lecture6.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lecture6.Helpers;

namespace Lecture6.Middleware
{
    public class CustomLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IDbService service)
        {
            if (context.Request != null)
            {
                string path = context.Request.Path; // /api/students
                string method = context.Request.Method; // GET, POST
                string queryString = context.Request.QueryString.ToString();
                string bodyStr = "";

                using(StreamReader reader=new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyStr = await reader.ReadToEndAsync();
                }
                // save to log file / log to database
                service.SaveLogData(path + 
                                    " " + method + 
                                    " " + queryString +
                                    " " + bodyStr); // includes logging to a file and to a database
            }

            if(_next!=null) await _next(context); //executes next middleware
        }
       
    }
}