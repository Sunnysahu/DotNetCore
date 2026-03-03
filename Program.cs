//using Middleware.Middleware;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Middleware.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<MyMiddleware>();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/name", () => "Hello Sunny from Map Get");

// app.Run is a Terminating Middleware and If You Use app.Run x 2 then last one won't run.

//app.Run(async (HttpContext context) =>
//{
//    await context.Response.WriteAsync("Terminating Middleware Run");
//});

//app.Run(async (HttpContext context) => {

//    await context.Response.WriteAsync("This Middleware Won't Run");
//});

// -----------------------------------------------------------------------

// app.Use is non-terminal middleware, it process request and go to next middleware. If we use app.Use() then we have a app.Run(). Standalone if you want to do, just make the await next() on top, as app.Map() will 

//app.Use(async (HttpContext context, RequestDelegate next) =>
//{
//    await next(context);
//    await context.Response.WriteAsync("Middleware 1 Executed ");
//});

// ------------------------------------------------------------------


// Here app.Use() Will Be executed then app.Run()
//app.Use(async (HttpContext context, RequestDelegate next) =>
//{
//    await context.Response.WriteAsync("Middleware 1 Executed + ");
//    await next(context);
//});

//app.Run(async (HttpContext context) =>
//{
//    await context.Response.WriteAsync("Middleware 2 Executed ");
//});

// -------------------------------------------------------------

// Some Properties of context
// https://localhost:7207/test/123?name=Sunny&role=developer&city=Jamshedpur

//app.Use(async (HttpContext context, RequestDelegate next) =>
//{
//    var request = context.Request;

//    // Enable body re-reading
//    request.EnableBuffering();

//    string bodyText = "";
//    using (var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true))
//    {
//        bodyText = await reader.ReadToEndAsync();
//        request.Body.Position = 0;
//    }

//    string output = $@"
//    ========= BASIC =========
//    Method: {request.Method}
//    Scheme: {request.Scheme}
//    Protocol: {request.Protocol}
//    IsHttps: {request.IsHttps}

//    ========= URL =========
//    Host: {request.Host}
//    Path: {request.Path}
//    PathBase: {request.PathBase}
//    QueryString: {request.QueryString}
//    FullUrl: {request.GetDisplayUrl()}

//    ========= HEADERS =========
//    User-Agent: {request.Headers["User-Agent"]}
//    Accept: {request.Headers["Accept"]}

//    ========= CONTENT =========
//    ContentType: {request.ContentType}
//    ContentLength: {request.ContentLength}
//    HasFormContentType: {request.HasFormContentType}

//    ========= QUERY PARAMETERS =========
//    {string.Join("\n", request.Query.Select(q => $"{q.Key} = {q.Value}"))}

//    ========= ROUTE VALUES =========
//    {string.Join("\n", request.RouteValues.Select(r => $"{r.Key} = {r.Value}"))}

//    ========= COOKIES =========
//    {string.Join("\n", request.Cookies.Select(c => $"{c.Key} = {c.Value}"))}

//    ========= SERVICES =========
//    RequestServices Available: {context.RequestServices != null}

//    ========= CANCELLATION =========
//    RequestAborted: {context.RequestAborted.IsCancellationRequested}

//    ========= BODY =========
//    {bodyText}
//";

//    context.Response.StatusCode = 200; // BEFORE writing
//    context.Response.ContentType = "text/plain";

//    var connection = context.Connection;

//    string ConnectionOutput = $@"
//    ========= CONNECTION INFO =========

//    ConnectionId: {connection.Id}

//    ----- CLIENT -----
//    RemoteIpAddress: {connection.RemoteIpAddress}
//    RemotePort: {connection.RemotePort}

//    ----- SERVER -----
//    LocalIpAddress: {connection.LocalIpAddress}
//    LocalPort: {connection.LocalPort}

//    ----- HTTPS / CERTIFICATE -----
//    ClientCertificate Present: {connection.ClientCertificate != null}
//    ";


//    await context.Response.WriteAsync(output + "\n" + ConnectionOutput);

//    //  DO NOT call next here
//});

//------------------------------------------------------------------------------------

// A General Code

// https://localhost:7207/?name=sunny&age-26

//app.Run(async (HttpContext context) =>
//{
//    int code = context.Response.StatusCode = 200;

//    string path = context.Request.Path;

//    string method = context.Request.Method;

//    string protocol = context.Request.Protocol;

//    var head = context.Request.Headers;

//    var Body = context.Request.Body;

//    var RouteValues = context.Request.RouteValues;

//    var ContentType = context.Request.ContentType;

//    var q = context.Request.QueryString;

//    string response = $"Hello Sunny from Run method. Path: {path}, Method: {method}, Status Code: {code}, Protocol : {protocol}, Header : {head}, Body : {Body} RouteValues : {RouteValues}, ContentType : {ContentType}, Query : {q}";

//    await context.Response.WriteAsync(response);
//});
// ---------------------------------------------------------------------

// How Middleware Works

// next give control to next app.Use() and after done, it retruns back...
// Here OUTPUT WILL BE Middleware 2 + Middleware 3 + Middleware 1 + Middleware 5 + Middleware 4 + 
//app.Use(async (context, next) =>
//{
//    await context.Response.WriteAsync("Middleware 2 + ");
//    await next(context);
//    await context.Response.WriteAsync("Middleware 4 + ");

//});

//app.Use(async (context, next) =>
//{
//    await context.Response.WriteAsync("Middleware 3 + ");
//    await next(context);
//    await context.Response.WriteAsync("Middleware 5 + ");
//});

//app.Run(async (context) =>
//{
//    await context.Response.WriteAsync("Middleware 1 + ");
//});

// -----------------------------------------------------------------------------

// Create Custom Middlewware -->  Create a Folder Middleware --> Create a Class MyMiddleware.cs --> Implement IMiddleware Interface --> Override InvokeAsync() Method --> Write Your Logic --> Register Middleware in Program.cs using app.UseMiddleware<MyMiddleware>() and add thus in top : builder.Services.AddTransient<MyMiddleware>();

// Middleware Flow : app.use() --> custom --> then app.Run() --> custom --> app.Run()

//app.Use(async (HttpContext context, RequestDelegate next) =>
//{
//    await context.Response.WriteAsync("MiddleWare Run + ");
//    await next(context);
//});

//app.UseMiddleware<MyMiddleware>();

//app.Run(async (HttpContext context) =>
//{
//    await context.Response.WriteAsync("Done + ");
//});

// -----------------------------------------------------------------------------

// Middleware extension : Make IApplicationBuilder Extension Method --> Create a Static Class --> Create a Static Method --> Return IApplicationBuilder --> Call app.UseMiddleware() Inside That Method and then call that method in Program.cs

// Now we can use app.UseMessage() instead of app.UseMiddleware<MyMiddleware>() and it will work same as before. This is just for better readability and to avoid code repetition if you have multiple middlewares.

//app.UseMessage(); // Custom Middleware with Extension Method. 

//app.Run(async (HttpContext context) =>
//{
//    await context.Response.WriteAsync("Done + ");
//});

// -----------------------------------------------------------------------------

//app.UseWhen()-- > It is used to conditionally execute middleware based on a predicate. If the predicate returns true, the specified middleware will be executed; otherwise, it will be skipped. This allows you to apply middleware only to certain requests based on criteria such as path, headers, or query parameters.

// This Hit URL --> https://localhost:7207/test?Email=sunny@gmail.com

//app.UseWhen(context => context.Request.Query.ContainsKey("Email"),
//    app =>
//    {
//        app.Use(async (context, next) =>
//        {
//            //var emailValue = context.Request.Query["Email"].ToString();
//            await context.Response.WriteAsync($"Email Query Parameter is Present {context.Request.Query["Email"].ToString()} ");
//            await next(context);
//        });
//        app.Use(async (context, next) =>
//        {
//            //var emailValue = context.Request.Query["Email"].ToString();
//            await context.Response.WriteAsync($"Password Query Parameter is Present {context.Request.Query["password"].ToString()} ");
//            await next(context);
//        });
//    });


//app.Run(async (HttpContext context) =>
//{
//    await context.Response.WriteAsync("Done + ");
//});

// -----------------------------------------------------------------------------

// Today, we don't use IMiddleware interface to create custom middleware, we just create a class and write our logic in Invoke() method and then register that class in Program.cs using app.UseMiddleware<YourClass>() and add thus in top : builder.Services.AddTransient<YourClass>(); This is just for better readability and to avoid code repetition if you have multiple middlewares.

// Go to Solution Explorer --> Right Click on Project --> Add --> New Folder --> Name it Middleware --> Right Click on Middleware Folder --> Add --> MiddlewareClass --> Name it MyMiddleware.cs --> Write Your Logic in Invoke() Method and then register that class in Program.cs using app.UseMiddleware<MyMiddleware>() and add thus in top : builder.Services.AddTransient<MyMiddleware>();


//app.UseCustomerMiddleware(); // Custom Middleware with Extension Method (NEW).

//app.Run(async (HttpContext context) =>
//{
//    await context.Response.WriteAsync("Done + ");
//});

// -----------------------------------------------------------------------------

















// -----------------------------------------------------------------------------

//await Task.Delay(5000);
// 20th Feb --> 2338 --> https://t.me/c/2870057718/213/270

app.Run();

