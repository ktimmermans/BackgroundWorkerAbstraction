using System;
using System.Collections.Generic;
using System.Text;

namespace Background.Interfaces
{
    public interface IBackgroundQueue
    {
        IEnumerable<TaskSettings> GetAllTasksForQueue();
    }
}
