using BuildingBlock.Base.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Text;

namespace ImageService.Infrastructure.Attributes
{
    public class UserRequestAttributeActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string authorizationHeader = context.HttpContext.Request.Headers["Authorization"];

            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                ISession session = GetSessionService(context);
                string token = TokenFormat(authorizationHeader);
                string? emailVal = GetServiceValues(context, token);

                AddSession(session, "email", emailVal);

                var tupleUrl = GetRequestUrl(context);
                Log.Information($"Not authorization person came the request !");
                Log.Information($"Request Url : /{tupleUrl.controller}/{tupleUrl.action}");
            }
            else
            {
                var tupleUrl = GetRequestUrl(context);
                Log.Information($"Not authorization person came the request !");
                Log.Information($"Request Url : /{tupleUrl.controller}/{tupleUrl.action}");
            }
        }

        private (string controller, string action) GetRequestUrl(ActionExecutingContext context)
        {
            var controllerName = context.RouteData.Values["controller"];
            var actionName = context.RouteData.Values["action"];
            return (controllerName.ToString(), actionName.ToString());
        }

        private string GetServiceValues(ActionExecutingContext context, string token)
        {
            var sp = context.HttpContext.RequestServices;
            var jwtService = sp.GetRequiredService<ITokenService>();
            var email = jwtService.GetEmailWithToken(token);
            return email;
        }

        private ISession GetSessionService(ActionExecutingContext context)
            => context.HttpContext.Session;

        private string TokenFormat(string authorizationHeader) => authorizationHeader.Substring(7);

        private void AddSession(ISession session, string name, string email)
        {
            byte[] emailBytes = Encoding.UTF8.GetBytes(email);
            session.Set(name, emailBytes);
        }
    }
}
