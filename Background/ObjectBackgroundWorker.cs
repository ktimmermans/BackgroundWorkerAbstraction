using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Background.Interfaces;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Background
{
    public abstract class ObjectBackgroundWorker<T> : BackgroundService
    {
        private readonly IObjectBackgroundQueue<T> _queue;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ObjectBackgroundWorker<T>> _logger;

        public ObjectBackgroundWorker(IObjectBackgroundQueue<T> queue, IServiceScopeFactory scopeFactory,
            ILogger<ObjectBackgroundWorker<T>> logger)
        {
            _queue = queue;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        /// <summary>
        /// Method to be overridden in order to actually process the task
        /// </summary>
        /// <param name="scope">A scope from which to retreive services needed for processing</param>
        /// <param name="itemToProcess">The item that is supposed to be processed</param>
        /// <returns>Nothing</returns>
        public abstract Task RunTaskWithItem(IServiceScope scope, T itemToProcess);

        /// <summary>
        /// Method to be overridden in order to process an error while processing the task
        /// </summary>
        /// <param name="scope">A scope from which to retreive services needed for processing</param>
        /// <param name="ex">The exception that was thrown</param>
        /// <returns>Nothing</returns>
        public abstract Task ProcessTaskException(IServiceScope scope, Exception ex);

        /// <summary>
        /// Function executed on each iteration
        /// </summary>
        /// <param name="stoppingToken">cancellation token used to interrupt running</param>
        /// <returns>Nothing</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("{Type} is now running in the background.", nameof(BackgroundWorker));

            await BackgroundProcessing(stoppingToken);
        }

        /// <summary>
        /// Stops running executions by setting the cancellation token
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Nothing</returns>
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogCritical(
                "The {Type} is stopping due to a host shutdown, queued items might not be processed anymore.",
                nameof(BackgroundWorker)
            );

            return base.StopAsync(cancellationToken);
        }

        /// <summary>
        /// Function called by ExecuteAsync in order to create a scope and call RunTask to process the task
        /// calls ProcessTaskException when an error occurs
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns>Nothing</returns>
        protected virtual async Task BackgroundProcessing(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var itemToProcess = _queue.Dequeue();

                    if (itemToProcess == null) continue;

                    using (var scope = _scopeFactory.CreateScope())
                    {
                        await this.RunTaskWithItem(scope, itemToProcess);
                    }

                    if (typeof(TaskSettings) == typeof(TaskSettings))
                    {
                        await Task.Delay(((TaskSettings)(object)itemToProcess).DelayMilliSeconds, stoppingToken);
                    }
                    else
                    {
                        await Task.Delay(500, stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        await this.ProcessTaskException(scope, ex);
                    }
                    await Task.Delay(500, stoppingToken);
                }
            }
        }
    }
}
