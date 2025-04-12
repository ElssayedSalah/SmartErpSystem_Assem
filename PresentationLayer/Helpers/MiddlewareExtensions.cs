using Microsoft.AspNetCore.Builder;
using PresentationLayer.Middlewares;

namespace PresentationLayer.Helpers
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder RunActivationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ActivationMiddleware>();
        }
    }
}
