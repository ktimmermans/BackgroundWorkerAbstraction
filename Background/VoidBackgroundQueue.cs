using Background.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Background
{
    public class VoidBackgroundQueue<TaskSettings> : IObjectBackgroundQueue<TaskSettings>
    {
        private readonly ConcurrentQueue<TaskSettings> _items = new ConcurrentQueue<TaskSettings>();

        /// <summary>
        /// Enqueue a task that is supposed to process a task that doesnt return an item
        /// </summary>
        /// <param name="item">A task that is supposed to be queued for processing</param>
        public void Enqueue(TaskSettings taskSettings)
        {
            _items.Enqueue(taskSettings);
        }

        /// <summary>
        /// Try to dequeue a task that is supposed to process
        /// </summary>
        /// <returns>A task waiting to be processed</returns>
        public TaskSettings Dequeue()
        {
            var success = _items.TryDequeue(out var workItem);

            return success
                ? workItem
                : default(TaskSettings);
        }
    }
}
