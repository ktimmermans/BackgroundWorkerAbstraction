using BackgroundWorker.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackgroundWorker
{
    public class EnforcedTaskSetting : TaskSettings
    {
        public override int DelayMilliSeconds { get; set; }
        public override string TaskName { get; set; }
    }
}
