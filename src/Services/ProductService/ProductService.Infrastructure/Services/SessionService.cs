using Microsoft.AspNetCore.Http;
using ProductService.Application.Abstractions;
using ProductService.Application.Exceptions;
using System.Text;

namespace ProductService.Infrastructure.Services
{
    public class SessionService<T> : ISessionService<T> where T : notnull
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetSessionValue(string key)
        {
            byte[] sessionData = _httpContextAccessor.HttpContext.Session.Get(key);

            if (sessionData != null)
            {
                try
                {
                    var jsonString = Encoding.UTF8.GetString(sessionData);
                    return jsonString;
                }
                catch (Exception ex)
                {
                    Serilog.Log.Error("Session ERROR : " + ex.Message);
                }
            }
            else
                throw new PiplineValidationError("Session not found the key value");

            return string.Empty;
        }
    }
}
