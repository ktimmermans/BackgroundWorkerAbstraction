using Backgroundworker.Exceptions;
using BackgroundWorker.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public int GetTotalAmountOfSpecificTask(Type taskType)
        {
            var backgroundTask = this._backgroundServices.FirstOrDefault(x => x.GetTypeOfQueue() == taskType);

            if (backgroundTask == null)
            {
                throw new UnknownQueueException($"No queue for type: {taskType} was found");
            }

            return backgroundTask.GetAmountOfTasks();
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
            var backgroundTask = this._backgroundServices.FirstOrDefault(x => x.GetTypeOfQueue() == taskType);

            if (backgroundTask == null)
            {
                throw new UnknownQueueException($"No queue for type: {taskType} was found");
            }


            return backgroundTask.GetAllTasksForQueue();
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
                throw new UnknownQueueException($"Task could not be added because its type was unknown, Did you register the correct provider?");
            }
        }
    }
}