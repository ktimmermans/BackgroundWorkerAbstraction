using Background.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Background
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddBackgroundQueue<T>(this IServiceCollection services) where T : class
        {
            services.AddSingleton<IObjectBackgroundQueue<T>, ObjectBackgroundQueue<T>>();

            // Add reference so that the manager can find them
            services.AddSingleton<IBackgroundQueue>(x => x.GetRequiredService<IObjectBackgroundQueue<T>>());

            return services;
        }
    }
}
