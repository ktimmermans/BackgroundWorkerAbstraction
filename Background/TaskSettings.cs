namespace Background
{
    public abstract class TaskSettings
    {
        public abstract int DelayMilliSeconds { get; set; }

        public abstract string TaskName { get; set; }

        public int GetDelayMilliSeconds()
        {
            return this.DelayMilliSeconds;
        }
    }
}
