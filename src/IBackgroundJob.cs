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
    /// A background job
    /// </summary>
    public interface IBackgroundJob
    {
        /// <summary>
        /// Executes the long running operation
        /// </summary>
        /// <param name="serviceProvider">IServiceProvider interface</param>
        /// <param name="cancellationToken">Indicates that the long running operation should be stopped.</param>
        /// <returns>A task that represents the long running operations.</returns>
        Task ExecuteAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken);
    }
}