using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using People.Api.Helpers;
using Swashbuckle.AspNetCore.Swagger;
using People.Models.Common;
using People.Models.Settings;
using People.MSSql;
using ServicesLib;
using Utilities;

namespace People.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IServiceProvider ServiceProvider;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var sqlConnectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<PeopleDbContext>(
                options => options.UseSqlServer(sqlConnectionString), ServiceLifetime.Scoped
            );


            services.AddControllers()
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    opt.JsonSerializerOptions.WriteIndented = true;
                    opt.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
            

            services.AddSingleton<IConfiguration>(Configuration);


            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "People API",
                        Description = "A simple example Web API",
                        Contact = new OpenApiContact()
                        {
                            Name = "Name",
                            Email = string.Empty,
                            //Url = "http://test.test"
                        }
                    }
                );
            });

            services.ConfigureSwaggerGen(options =>
            {
                options.CustomSchemaIds(x => x.FullName);
            });

            services.RegisterServices(Configuration);

            services.Configure<ApiBehaviorOptions>(o =>
            {
                o.InvalidModelStateResponseFactory = actionContext =>
                {
                    var keyList = string.Join(",", actionContext.ModelState.Keys);

                    return ControllersHelper.ReturnContentResult(SerializerJson.SerializeObjectToJsonString(ResponseHelper.ReturnBadRequest($"ModelState.Keys: {keyList}")));
                };
            });

            services.AddLogging(builder =>
            {
                builder.AddConfiguration(Configuration.GetSection("Logging"))
                    .AddConsole()
                    .AddDebug();
            });

            services.Configure<WSettings>(Configuration);
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, PeopleDbContext dbContext)
        {

            try
            {
                dbContext.Database.Migrate();
            }
            catch (Exception exception)
            {
                LoggerStatic.Logger.Error("Startup Configure exception: " + exception);
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseExceptionHandler(
                    options =>
                    {
                        options.Run(
                            async context =>
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                context.Response.ContentType = "application/json";
                                var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                                if (exceptionHandlerFeature != null)
                                {
                                    LoggerStatic.Logger.Error("UseExceptionHandler Error: " + exceptionHandlerFeature.Error);
                                    var text = SerializerJson.SerializeObjectToJsonString(new ResponseError(ErrorType.InternalError));
                                    await context.Response.WriteAsync(text).ConfigureAwait(false);
                                }
                            });
                    }
                );
            }
            else
            {
                app.UseExceptionHandler(
                    options =>
                    {
                        options.Run(
                            async context =>
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                context.Response.ContentType = "application/json";
                                var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                                if (exceptionHandlerFeature != null)
                                {
                                    LoggerStatic.Logger.Error("UseExceptionHandler Error: " + exceptionHandlerFeature.Error);
                                    var text = SerializerJson.SerializeObjectToJsonString(new ResponseError(ErrorType.InternalError));
                                    await context.Response.WriteAsync(text).ConfigureAwait(false);
                                }
                            });
                    }
                );
            }

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "defaultApi",
                    pattern: "api/{controller}/{action}/{id?}");
            }
            );

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "People API V1");
            });
        }
    }
}
