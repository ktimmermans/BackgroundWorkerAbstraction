using System;

namespace BackgroundWorker.Abstractions
{
    public abstract class TaskSettings
    {
        private DateTime _addedDate { get; set; }

        public abstract int DelayMilliSeconds { get; set; }

        public abstract string TaskName { get; set; }

        public void SentToQueue()
        {
            this._addedDate = DateTime.Now;
        }

        public DateTime GetNextRunTime()
        {
            return this._addedDate.AddMilliseconds(this.DelayMilliSeconds);
        }

        public string GetName()
        {
            return string.IsNullOrEmpty(TaskName) ? this.ToString() : this.TaskName;
        }
    }
}
