﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Background.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Background
{
    public abstract class VoidBackgroundWorker<TaskSettings> : ObjectBackgroundWorker<TaskSettings>
    {
        public VoidBackgroundWorker(IObjectBackgroundQueue<TaskSettings> queue, IServiceScopeFactory scopeFactory,
            ILogger<VoidBackgroundWorker<TaskSettings>> logger) : base(queue, scopeFactory, logger)
        {
        }

        /// <summary>
        /// Method to be overridden in order to actually process the task
        /// </summary>
        /// <param name="scope">A scope from which to retreive services needed for processing</param>
        /// <returns>Nothing</returns>
        public abstract Task RunTask(IServiceScope scope);

        public override async Task RunTaskWithItem(IServiceScope scope, TaskSettings itemToProcess)
        {
            await this.RunTask(scope);
        }
    }
}