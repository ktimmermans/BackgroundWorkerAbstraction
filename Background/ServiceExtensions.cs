using BackgroundWorker.Abstractions;
using BackgroundWorker.TaskManager;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BackgroundWorker
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

        public static IServiceCollection AddTaskManager(this IServiceCollection services)
        {
            services.AddTransient<IBackgroundTaskManager, BackgroundTaskManager>();

            return services;
        }
    }
}
