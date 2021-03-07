using System;
using System.Collections.Generic;
using System.Text;

namespace BackgroundWorker.Abstractions
{
    public interface IBackgroundQueue
    {
        IEnumerable<TaskSettings> GetAllTasksForQueue();
    }
}
