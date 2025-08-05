using System.Diagnostics;

namespace API_Demo.Middleware
{
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;
        public RequestTimingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            await _next(context);
            stopwatch.Stop();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{context.Request.Method,-6} => {context.Request.Path,-25} => Took {stopwatch.ElapsedMilliseconds,6} ms");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
