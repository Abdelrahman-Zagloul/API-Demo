using System.Collections.Concurrent;

namespace API_Demo.Middleware;
public class RateLimitPerIpMiddleware
{
    private readonly RequestDelegate _next;
    private static ConcurrentDictionary<string, (int Count, DateTime StartTime)> _ipRequests = new();

    public RateLimitPerIpMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        DateTime now = DateTime.UtcNow;

        if (!_ipRequests.ContainsKey(ip))
        {
            _ipRequests[ip] = (1, now);
        }
        else
        {
            var (count, startTime) = _ipRequests[ip];

            if ((now - startTime).TotalSeconds > 10)
            {
                _ipRequests[ip] = (1, now);
            }
            else
            {
                count++;
                if (count > 5)
                {
                    context.Response.StatusCode = 429;
                    await context.Response.WriteAsync("Too many requests from your IP. Try again later.");
                    return;
                }
                _ipRequests[ip] = (count, startTime);
            }
        }

        await _next(context);
    }
}
