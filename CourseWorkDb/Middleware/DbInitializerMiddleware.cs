using CourseWorkDb.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CourseWorkDb.Middleware
{
    public class DbInitializerMiddleware
    {
        private readonly RequestDelegate _next;

        public DbInitializerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext, RationingDbContext db)
        {
            DbInitializer.Initialize(db);
            return _next(httpContext);
        }
    }

    public static class DbMiddlewareExtensions
    {
        public static IApplicationBuilder UseInitializerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DbInitializerMiddleware>();
        }
    }
}
