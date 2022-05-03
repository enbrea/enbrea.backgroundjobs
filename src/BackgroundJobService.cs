#region ENBREA - Copyright (C) 2022 STÜBER SYSTEMS GmbH
/*    
 *    ENBREA 
 *    
 *    Copyright (C) 2022 STÜBER SYSTEMS GmbH
 *
 *    Licensed under the MIT License, Version 2.0. 
 * 
 */
#endregion

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Enbrea.BackgroundJobs
{
    /// <summary>
    /// A hosted service for dequeuing and executing jobs from a background queue
    /// </summary>
    /// <typeparam name="T">Type of background job</typeparam>
    public class BackgroundJobService<T> : BackgroundService
        where T : IBackgroundJob
    {
        private readonly BackgroundJobQueue<T> _jobQueue;
        private readonly ILogger<IBackgroundJob> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundJobService<T>"/> class.
        /// </summary>
        /// <param name="taskQueue">Pointer to job queue</param>
        /// <param name="serviceScopeFactory">IServiceScopeFactory interface</param>
        /// <param name="logger">Logger interface</param>
        public BackgroundJobService(BackgroundJobQueue<T> jobQueue, IServiceScopeFactory serviceScopeFactory, ILogger<IBackgroundJob> logger)
        {
            _logger = logger;
            _jobQueue = jobQueue;
            _serviceScopeFactory = serviceScopeFactory;
        }

        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>Pointer to task object</returns>
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("BackgroundJobService started");
            return base.StartAsync(cancellationToken);
        }

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process has been aborted.</param>
        /// <returns>Pointer to task object</returns>
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("BackgroundJobService stopped");
            return base.StopAsync(cancellationToken);
        }

        /// <summary>
        /// Implements a long running operation: Dequeuing and starting of jobs in sequential order.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the long running operation should be stopped.</param>
        /// <returns>A task that represents the long running operations.</returns>
        protected async override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            { 
                while (!cancellationToken.IsCancellationRequested)
                {
                    var job = await _jobQueue.DequeueAsync(cancellationToken);
                    try
                    {
                        using (var scope = _serviceScopeFactory.CreateScope())
                        {
                            _logger.LogDebug("BackgroundJob started");
                            await job.ExecuteAsync(scope.ServiceProvider, cancellationToken);
                            _logger.LogDebug("BackgroundJob finished");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "BackgroundJob failed");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in BackgroundJobService");
            }
        }
    }
}