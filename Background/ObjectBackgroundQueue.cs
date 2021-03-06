using Background.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Background
{
    public class ObjectBackgroundQueue<T> : IObjectBackgroundQueue<T> where T : class
    {
        private readonly ConcurrentQueue<T> _items = new ConcurrentQueue<T>();

        /// <summary>
        /// Enqueue a task that is supposed to process an object
        /// </summary>
        /// <param name="item">A task that is supposed to return an object to be queued for processing</param>
        public void Enqueue(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            if (item.GetType().BaseType == typeof(TaskSettings))
            {
                ((TaskSettings)(object)item).SentToQueue();
            }
            _items.Enqueue(item);
        }

        /// <summary>
        /// Try to dequeue a task that is supposed to process an object
        /// </summary>
        /// <returns>A task waiting to be processed</returns>
        public T Dequeue()
        {
            var success = _items.TryDequeue(out var workItem);

            return success
                ? workItem
                : null;
        }

        /// <summary>
        /// Return all tasks current in the queue (a copy)
        /// </summary>
        /// <returns>A list of copies of the tasks currently in the queue</returns>
        public IEnumerable<TaskSettings> GetAllTasksForQueue()
        {
            foreach (var t in this._items.ToArray())
            {
                if (t.GetType().BaseType == typeof(TaskSettings))
                {
                    yield return (TaskSettings)(object)t;
                }
                else
                {
                    yield return null;
                }
            }
        }
    }
}
