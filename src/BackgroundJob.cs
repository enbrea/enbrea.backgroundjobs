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

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Enbrea.BackgroundJobs
{
    /// <summary>
    /// A generic background job
    /// </summary>
    public class BackgroundJob : IBackgroundJob
    {
        private readonly Func<IServiceProvider, CancellationToken, Task>  _workItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundJob"/> class.
        /// </summary>
        /// <param name="workItem">A work item which represents a long running operation</param>
        public BackgroundJob(Func<IServiceProvider, CancellationToken, Task> workItem)
        {
            _workItem = workItem;
        }

        /// <summary>
        /// Executes the long running operation
        /// </summary>
        /// <param name="serviceProvider">IServiceProvider interface</param>
        /// <param name="cancellationToken">Indicates that the long running operation should be stopped.</param>
        /// <returns>A task that represents the long running operations.</returns>
        public async Task ExecuteAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            await _workItem(serviceProvider, cancellationToken);
        }
    }
}