using BackgroundWorker.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace BackgroundWorker.TaskManager
{
    public class BackgroundTaskManager : IBackgroundTaskManager
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEnumerable<IBackgroundQueue> _backgroundServices;

        public BackgroundTaskManager(
            IServiceProvider serviceProvider,
            IEnumerable<IBackgroundQueue> backgroundServices)
        {
            this._backgroundServices = backgroundServices;
            this._serviceProvider = serviceProvider;
        }

        public int GetTotalAmountOfCurrentBackgroundTasks(Type taskType)
        {
            int amountOfTasks = 0;

            foreach (var bs in this._backgroundServices)
            {
                if (bs.GetTypeOfQueue() == taskType)
                {
                    amountOfTasks += bs.GetAmountOfTasks();
                }
            }

            return amountOfTasks;
        }

        public int GetTotalAmountOfCurrentBackgroundTasks()
        {
            int amountOfTasks = 0;

            foreach (var bs in this._backgroundServices)
            {
                amountOfTasks += bs.GetAmountOfTasks();
            }

            return amountOfTasks;
        }

        public IEnumerable<TaskSettings> GetCurrentBackgroundTasksForQueue(Type taskType)
        {
            List<TaskSettings> queuedTasks = new List<TaskSettings>();
            foreach (var bs in this._backgroundServices)
            {
                if (bs.GetTypeOfQueue() == taskType)
                {
                    queuedTasks.AddRange(bs.GetAllTasksForQueue());
                }
            }

            return queuedTasks;
        }

        public IEnumerable<TaskSettings> GetCurrentBackgroundTasks()
        {
            List<TaskSettings> queuedTasks = new List<TaskSettings>();
            foreach (var bs in this._backgroundServices)
            {
                queuedTasks.AddRange(bs.GetAllTasksForQueue());
            }

            return queuedTasks;
        }

        public void AddTask<T>(T objectTask) where T : class
        {
            try
            {
                var requiredTaskQueue = this._serviceProvider.GetRequiredService<IObjectBackgroundQueue<T>>();
                requiredTaskQueue.Enqueue(objectTask);
            }
            catch (Exception ex)
            {
                throw new Exception($"Task could not be added because its type was unknown, Did you register the correct provider?");
            }
        }
    }
}