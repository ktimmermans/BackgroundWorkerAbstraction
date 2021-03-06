using Background.Interfaces;
using System;
using System.Collections.Generic;

namespace Background
{
    public class BackgroundTaskManager : IBackgroundTaskManager
    {
        private readonly IEnumerable<IBackgroundQueue> _backgroundServices;

        public BackgroundTaskManager(
            IEnumerable<IBackgroundQueue> backgroundServices)
        {
            this._backgroundServices = backgroundServices;
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
    }
}
