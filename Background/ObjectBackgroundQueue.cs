using Background.Interfaces;
using System;
using System.Collections.Concurrent;

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
    }
}
