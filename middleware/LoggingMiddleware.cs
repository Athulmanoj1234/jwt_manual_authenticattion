using System.Transactions;

namespace jwtmanualauthentication.middleware
{
    public class LoggingMiddleware(RequestDelegate next) 
    {
        //If you want a class to act as middleware, it must have a public method called Invoke or InvokeAsync.The framework will automatically call this method for each HTTP request.
        public async Task InvokeAsync(HttpContext context) {
            Console.WriteLine("this is the structured sample middlewares" + context.Request.Path);
            await next(context);
            Console.WriteLine($"this is the middleware passed: { context.Request }");
        }
    }
}
