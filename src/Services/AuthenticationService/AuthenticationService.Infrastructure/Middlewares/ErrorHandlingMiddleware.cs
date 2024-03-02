using AuthenticationService.Application.Exceptions;
using BuildingBlock.Base.Extensions;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace AuthenticationService.Infrastructure.Middlewares
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
            catch (RepositoryErrorException ex)
            {
                await HandleExceptionAsync(context, ex, (HttpStatusCode)400);
            }
            catch (ServiceErrorException ex)
            {
                await HandleExceptionAsync(context, ex, (HttpStatusCode)400);
            }
            catch (UserNotExistsException ex)
            {
                await HandleExceptionAsync(context, ex, (HttpStatusCode)404);
            }
            catch (ValueNullException ex)
            {
                await HandleExceptionAsync(context, ex, (HttpStatusCode)404);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, (HttpStatusCode)500);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex, HttpStatusCode statusCode = (HttpStatusCode)500)
        {
            Serilog.Log.Error("ERROR MESSAGE : " + ex.Message);
            //var code = HttpStatusCode.InternalServerError;
            var code = statusCode;
            var result = (new { error = $"Error appeared when maked process ! + {ex.Message}" }).SerialJson();
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
