using System.Text;

namespace Middleware.Middleware
{
    public class MyMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await context.Response.WriteAsync("Custom Middleware 1 + ");
            await next(context);
            StringBuilder statusCode = new StringBuilder();
            statusCode.Append(context.Response.StatusCode.ToString());
            Console.WriteLine("Status Code : " + statusCode);
            await context.Response.WriteAsync("Custom Middleware 2 + ");
            Console.WriteLine("Hello World");

            string[] names = { "Sunny", "John", "Alice", "Bob" };

            string message = names.Contains("Sunny") ? "Sunny is present" : "Sunny is not present";
            await next(context);
        }
    }
    public static class Middlewares
    {
        public static IApplicationBuilder UseMessage(this IApplicationBuilder app)
        {
            return app.UseMiddleware<MyMiddleware>(); ;
        }
    }
}






