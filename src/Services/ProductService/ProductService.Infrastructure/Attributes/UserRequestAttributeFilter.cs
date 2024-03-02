using AuthenticationService.Domain.Aggregate;
using BuildingBlock.Base.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace ProductService.Infrastructure.Attributes
{
    public class UserRequestAttributeActionFilter : ActionFilterAttribute
    {
        public async override void OnActionExecuting(ActionExecutingContext context)
        {
            string authorizationHeader = context.HttpContext.Request.Headers["Authorization"];

            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                ISession session = GetSessionService(context);
                string token = TokenFormat(authorizationHeader);
                //var serviceValues = await GetServiceValues(context, token);

                var sp = context.HttpContext.RequestServices;
                var tokenService = sp.GetRequiredService<ITokenService>();

                AddSession(session, "email", tokenService.GetEmailWithToken(token));

                var tupleUrl = GetRequestUrl(context);
                Serilog.Log.Information($"Not authorization person came the request !");
                Serilog.Log.Information($"Request Url : /{tupleUrl.controller}/{tupleUrl.action}");
            }
            else
            {
                var tupleUrl = GetRequestUrl(context);
                Serilog.Log.Information($"Not authorization person came the request !");
                Serilog.Log.Information($"Request Url : /{tupleUrl.controller}/{tupleUrl.action}");
            }
        }

        private (string controller, string action) GetRequestUrl(ActionExecutingContext context)
        {
            var controllerName = context.RouteData.Values["controller"];
            var actionName = context.RouteData.Values["action"];
            return (controllerName.ToString(), actionName.ToString());
        }

        private async Task<(string email, User user)> GetServiceValues(ActionExecutingContext context, string token)
        {
            var sp = context.HttpContext.RequestServices;
            var jwtService = sp.GetRequiredService<ITokenService>();
            var userModel = await jwtService.GetUserWithTokenAsync(token);
            return (userModel.Email, User.Create(userModel.FullName, userModel.Email, default, default, default));
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
