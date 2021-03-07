using BackgroundWorker.Abstractions;
using System.Collections.Generic;

namespace BackgroundWorker.TaskManager
{
    public interface IBackgroundTaskManager
    {
        IEnumerable<TaskSettings> GetCurrentBackgroundTasks();

        void AddTask<T>(T objectTask) where T : class;
    }
}
