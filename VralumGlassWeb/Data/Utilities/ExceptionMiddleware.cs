using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace VralumGlassWeb.Data.Utilities
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostingEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, IHostingEnvironment env, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _env = env ?? throw new ArgumentNullException(nameof(env));
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                _logger.LogError($"{e}");

                if (context.Request.IsAjaxRequest())
                {
                    await HandleJsonExceptionAsync(context, e);
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Moved; ;
                    context.Response.Headers[HeaderNames.Location] = "/Error";
                }
            }
        }

        private Task HandleJsonExceptionAsync(HttpContext context, Exception e)
        {
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                Message = !_env.IsDevelopment() ? "Internal Server Error." : e.Message
            }.ToString());
        }
    }
}
