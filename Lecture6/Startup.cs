using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lecture6.Middleware;
using Lecture6.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Lecture6.Helpers;

namespace Lecture6
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            DataLogger.ClearLog(DataLogger.requestsLogFile);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddScoped<IDbService, DbService>();

            //Swagger documentation
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Student API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbService service)
        {
            service.ClearLog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            
            string indexName = "Index";

            app.Use(async (context, next) =>
            {
                if (!context.Request.Headers.ContainsKey(indexName))
                {
                    context.Response.StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("No Index found in header");
                    return;
                }
                else 
                {
                    string index = context.Request.Headers[indexName].ToString();

                    // Check if this student is in the database
                    if (service.GetStudentBy_IndexNumber(index) == null)
                    {
                        // 401 if not found
                        context.Response.StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Student with this indexNumber not found");
                        return;
                    }
                }
                await next();
            });
            

            //Documentation
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Student API V1");
            });

            // Request -> Index: s1234
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseMiddleware<CustomLoggingMiddleware>();
            
            //Inline middleware (conditional)
            app.UseWhen(context => context.Request.Path.ToString().Contains("secret"), app => app.Use(async (context, next)=>
            {
                if (!context.Request.Headers.ContainsKey("Index"))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Index number required");
                    return;
                }

                string index = context.Request.Headers["Index"].ToString();
                //stateless
                //check in db if this index exists
                var st = service.GetStudentByIndex(index);
                if (st == null)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("Incorrect Index number");
                    return;
                }

                await next(); //calls the next middleware
            }));


            app.UseRouting();  // api/students GET -> StudentsController, GetStudents

            //app.UseAuthorization(); -- dont have permissions for this resource

            app.UseEndpoints(endpoints => // new StudentsController(?).GetStudents();
            {
                endpoints.MapControllers();
            });

        }
    }
}
