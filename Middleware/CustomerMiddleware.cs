using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Middleware.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CustomerMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            await httpContext.Response.WriteAsync("Hello From Customer Middleware (NEW) 1 + ");
            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CustomerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomerMiddleware>();
        }
    }
}
