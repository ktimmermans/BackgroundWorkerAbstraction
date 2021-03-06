using System.Collections.Generic;

namespace Background.Interfaces
{
    public interface IBackgroundTaskManager
    {
        IEnumerable<TaskSettings> GetCurrentBackgroundTasks();
    }
}
