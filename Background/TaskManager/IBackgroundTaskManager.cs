using BackgroundWorker.Abstractions;
using System;
using System.Collections.Generic;

namespace BackgroundWorker.TaskManager
{
    public interface IBackgroundTaskManager
    {
        int GetTotalAmountOfSpecificTask(Type taskType);

        int GetTotalAmountOfCurrentBackgroundTasks();

        IEnumerable<TaskSettings> GetCurrentBackgroundTasksForQueue(Type taskType);

        IEnumerable<TaskSettings> GetCurrentBackgroundTasks();

        void AddTask<T>(T objectTask) where T : class;
    }
}
