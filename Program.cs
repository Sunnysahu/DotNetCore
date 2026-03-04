//using Middleware.Middleware;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Middleware.Middleware;
using System.Text;
using System.Xml.Linq;

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

// Routing --> It is a way to map incoming HTTP requests to specific handlers based on the URL and HTTP method. In NET Core, you can define routes using app.Map(), app.MapGet(), app.MapPost(), app.MapPut(), app.MapDelete(), app.MapPatch(), app.MapHead(), app.MapOptions(), app.MapMethods(), app.MapFallback(), app.MapFallbackToFile(),app.MapGroup(). These methods take a URL pattern and a handler function that will be executed when a request matches the pattern and HTTP method.

// app.Map() -> Creates a branch in the request pipeline based on a path prefix. Not tied to a specific HTTP method.
// app.MapGet() -> Maps GET requests to a specific path. Only handles GET requests.
// app.MapPost() -> Maps POST requests to a specific path. Only handles POST requests.
// app.MapPut() -> Maps PUT requests to a specific path. Only handles PUT requests.
// app.MapDelete() -> Maps DELETE requests to a specific path. Only handles DELETE requests.
// app.MapPatch() -> Maps PATCH requests to a specific path. Only handles PATCH requests.
// app.MapHead() -> Maps HEAD requests to a specific path. Only handles HEAD requests.
// app.MapOptions() -> Maps OPTIONS requests to a specific path. Only handles OPTIONS requests.
// app.MapMethods() -> Maps requests to a specific path based on specified HTTP methods. Allows handling multiple HTTP methods for the same path.
// app.MapFallback() -> Defines a fallback route that will be executed if no other routes match the incoming request. Useful for handling 404 Not Found scenarios or providing a default response.
// app.MapFallbackToFile() -> Defines a fallback route that serves a static file if no other routes match the incoming request. Useful for serving a default HTML page or other static content when a route is not found.
// app.MapGroup() -> Allows grouping multiple routes under a common path prefix. This is useful for organizing related routes together and applying common middleware or configuration to the group.

//app.Map("/home/index", () => "Hello Sunny from Map! ");

// Route Parameters --> Route parameters use `{}` to capture values from the URL, like `/users/{id}`. They pass those values to the handler function. They make routes dynamic without creating separate routes for each value. You can also use multiple parameters in a single route.

// This Hit URL --> https://localhost:7207/product/details/sunny
// Single Route Parameter
//app.Map("product/details/{name}", async (context) =>
//{
//    //string name = context.Request.RouteValues["name"]?.ToString() ?? "Unknown";
//    string name = Convert.ToString(context.Request.RouteValues["name"])?.ToString() ?? "Unknown";
//    await context.Response.WriteAsync($"Name : {name}");

//});

// This Hit URL --> https://localhost:7207/product/sunny/jpg
// Multiple Route Parameters
//app.Map("product/{filename}/{extension}", async (context) =>
//{
//    string filename = context.Request.RouteValues["filename"]?.ToString() ?? "Unknown";
//    string extension = context.Request.RouteValues["extension"]?.ToString() ?? "Unknown";
//    await context.Response.WriteAsync($"Filename : {filename}, Extension : {extension}");
//});


// Optional Route Parameters --> You can make route parameters optional by adding a `?` after the parameter name in the route template. For example, `/users/{id?}` means that the `id` parameter is optional. If a request comes in without the `id`, the route will still match, and you can handle it accordingly in your handler function.

// This Hit URL --> https://localhost:7207/company/sunny/001
//app.Map("company/{name}/{employeeId?}", async (context) =>
//{
//    var conn = context.Request;

//    await context.Response.WriteAsync($"Name : {conn.RouteValues["name"]?.ToString() ?? "Unknown"} and Employee ID : {conn.RouteValues["employeeId"]?.ToString() ?? "Unknown"} ");
//});

// Default Values in Route Parameters --> You can provide default values for route parameters by using the `=` syntax in the route template. For example, `/users/{id=1}` means that if the `id` parameter is not provided in the URL, it will default to `1`. This allows you to have a fallback value for parameters that may not always be included in the request.

// This Hit URL --> /company --> /company/Sunny --> /company/Sunny/001 --> /company/001 --> /company/001/002 (This won't work because name is not optional but employeeId is optional)

//app.MapGet("company/{name=Alias}/{employeeId?}", async (context) =>
//{
//    var conn = context.Request;

//    await context.Response.WriteAsync($"Name : {conn.RouteValues["name"]?.ToString() ?? "Unknown"} and Employee ID : {conn.RouteValues["employeeId"]?.ToString() ?? "Unknown"} ");
//});


// Route Constraint --> Route constraints allow you to specify rules for route parameters, ensuring that they match certain criteria before the route is considered a match. Like int, alpha, length, range, regex etc. This helps in validating the incoming requests and ensures that the parameters meet the expected format or type before processing the request further.

// This Hit URL --> https://localhost:7207/home/index/123 --> This will only accept a number in the query name id. 
// This Hit URl -->  https://localhost:7207/home/index/sunny will give error


app.MapGet("home/index/{id:int}", async (context) =>
{
    await context.Response.WriteAsync($"ID : {context.Request.RouteValues["id"]}");
});

// This Hit URL -->  https://localhost:7207/home/5/99.99/Sunny/ABCDE/3fa85f64-5717-4562-b3fc-2c963f66afa6
// --> This will only accept a number in the query name id, a decimal number in price, a string with only alphabets in name, a string with length of 5 in code and a valid guid in uid. If any of these constraints are not met, it will give an error.

app.MapGet(
    "home/{id:int}/{price:decimal}/{name:alpha}/{code:length(5)}/{uid:guid}",
    (int id, decimal price, string name, string code, Guid uid, HttpContext context) =>
    {
        var userAgent = context.Request.Headers["User-Agent"].ToString();
        return $"ID: {id}, Price: {price}, Name: {name}, Code: {code}, GUID: {uid}, UserAgent : {userAgent}";
    });

// This Hit URL --> https://localhost:7207/home/10/25/Sunny/ABCDE/99.99/947dfefa-4ba9-49b7-81c4-b0ead06f2937
app.MapGet(
     "home/{id:int:min(1):max(100)}/{age:int}/{name:alpha:minlength(3):maxlength(10)}/{code:length(5):regex([A-Z]+)}/{price:decimal}/{uid:guid}",
 async (context) =>
    {
        var id = context.Request.RouteValues["id"];
        var age = Convert.ToInt16(context.Request.RouteValues["age"]);
        var name = context.Request.RouteValues["name"];
        var code = context.Request.RouteValues["code"];
        var price = context.Request.RouteValues["price"];
        var uid = context.Request.RouteValues["uid"];
        if (age > 60) 
        { 
            await context.Response.WriteAsync("Invalid Age");
            return;
        }
        await context.Response.WriteAsync($"ID: {id}, Age: {age}, Name: {name}, Code: {code}, Price: {price}, GUID: {uid}");
    });

// This Hit URL --> https://localhost:7207/test/25/2024-01-01/9999999999/12.5/123.456/true
app.MapGet(
    "test/{age:int:range(18,60)}/{date:datetime}/{bigNumber:long}/{f:float}/{d:double}/{status:bool}",
    (int age, DateTime date, long bigNumber, float f, double d, bool status) =>
    {
        return $"Age: {age}, Date: {date}, Long: {bigNumber}, Float: {f}, Double: {d}, Bool: {status}";
    });


// If no Route Match then this will Trigger
app.MapFallback(async (context) =>
{
    string path = context.Request.Path;
    await context.Response.WriteAsync(path + " Not Found. This is Fallback Route.");
});


// -----------------------------------------------------------------------------






















// -----------------------------------------------------------------------------

//await Task.Delay(5000);
// 21st Feb --> 0544 --> https://t.me/c/2870057718/213/285

app.Run();

