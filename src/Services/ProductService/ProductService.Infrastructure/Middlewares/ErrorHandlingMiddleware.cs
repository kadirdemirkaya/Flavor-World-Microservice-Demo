using BuildingBlock.Base.Extensions;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace ProductService.Infrastructure.Middlewares
{
    public class ErrorHandlingMiddleware

    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            Serilog.Log.Error("ERROR MESSAGE : " + ex.Message);
            var code = HttpStatusCode.InternalServerError;
            var result = JsonExtension.SerialJson(new { error = "Error appeared when maked process !" });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
