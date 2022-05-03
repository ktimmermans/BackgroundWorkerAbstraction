using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace BackgroundWorker.Abstractions
{
    public class ObjectBackgroundQueue<T> : IObjectBackgroundQueue<T> where T : class
    {
        private readonly ConcurrentDictionary<DateTime, T> _sortedItems = new ConcurrentDictionary<DateTime, T>();

        /// <summary>
        /// Enqueue a task that is supposed to process an object
        /// </summary>
        /// <param name="item">A task that is supposed to return an object to be queued for processing</param>
        public void Enqueue(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            if (this.IsTaskType())
            {
                this.ConvertToTaskSettings(item).SentToQueue();
                this._sortedItems.TryAdd(this.ConvertToTaskSettings(item).GetNextRunTime(), item);
            }
            else
            {
                this._sortedItems.TryAdd(DateTime.Now, item);
            }
        }

        /// <summary>
        /// Try to dequeue a task that is supposed to process an object
        /// </summary>
        /// <returns>A task waiting to be processed</returns>
        public T Dequeue()
        {
            var firstKey = this._sortedItems.OrderBy(x => x.Key).Select(x => x.Key).FirstOrDefault();
            if (this._sortedItems.Count > 0 && firstKey <= DateTime.Now)
            {
                var success = this._sortedItems.TryRemove(firstKey, out var itemToWork);
                return success ? itemToWork : null;
            }

            return null;
        }

        /// <summary>
        /// Return all tasks current in the queue (a copy)
        /// </summary>
        /// <returns>A list of copies of the tasks currently in the queue</returns>
        public IEnumerable<TaskSettings> GetAllTasksForQueue()
        {
            return this._sortedItems.OrderBy(x => x.Key).Select(x => this.ConvertToTaskSettings(x.Value));
        }

        /// <summary>
        /// Returns the total amount of tasks yet to be done over all queues
        /// </summary>
        /// <returns>an integer number of tasks to be done</returns>
        public int GetAmountOfTasks()
        {
            return this._sortedItems.Count;
        }

        /// <summary>
        /// Returns the object type of the queue
        /// </summary>
        /// <returns>The object type of the queue</returns>
        public Type GetTypeOfQueue()
        {
            return typeof(T);
        }

        private TaskSettings ConvertToTaskSettings<T>(T objectToConvert)
        {
            if (this.IsTaskType())
            {
                return (TaskSettings)(object)objectToConvert;
            }

            return new EnforcedTaskSetting { TaskName = objectToConvert.ToString() };
        }

        private bool IsTaskType()
        {
            return typeof(T).BaseType == typeof(TaskSettings);
        }
    }
}
