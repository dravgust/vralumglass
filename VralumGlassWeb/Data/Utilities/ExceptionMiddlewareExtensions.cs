using System;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace VralumGlassWeb.Data.Utilities
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void UseCustomException(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }

        public static void UseCustomExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        logger.LogError($"Something went wrong: {contextFeature.Error}");
                    }

                    if (context.Request.IsAjaxRequest())
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "application/json; charset=utf-8";

                        if (contextFeature != null)
                        {
                            await context.Response.WriteAsync(new ErrorDetails
                            {
                                StatusCode = context.Response.StatusCode,
                                Message = "Internal Server Error."
                            }.ToString());
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Moved; ;
                        context.Response.Headers[HeaderNames.Location] = "/Error";
                    }
                });
            });
        }
    }
}
