
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace ActionDetection.API.Middlewares
{
    public class AccesStatsMiddleware
    {
        private readonly RequestDelegate _next;

        public AccesStatsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            await _next(context);
            stopwatch.Stop();

            Console.WriteLine($"{context.Request.Headers["X-Forwarded-For"]} ({context.Connection.Id}): {context.Request.Path}{context.Request.QueryString} ({stopwatch.Elapsed.TotalMilliseconds}ms)");
        }
    }
}
