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

using System.Threading;
using System.Threading.Tasks;

namespace Enbrea.BackgroundJobs
{
    /// <summary>
    /// A background job queue
    /// </summary>
    public interface IBackgroundJobQueue<T> where T : IBackgroundJob
    {
        /// <summary>
        /// Gets the number of jobs contained in the queue.
        /// </summary>
        /// <returns>NUmber of jobs</returns>
        int GetCount();

        /// <summary>
        /// Removes and returns the job at the beginning of the queue. 
        /// </summary>
        /// <param name="cancellationToken">The CancellationToken token to observe.</param>
        /// <returns>Dequeued task</returns>
        /// <summary>
        /// If the queue is empty this function will waits until a new job is queued or the CancellationToken is signaled.
        /// </summary>
        Task<IBackgroundJob> DequeueAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Adds a job to the end of the queue.
        /// </summary>
        /// <param name="workItem">Pointer to work item</param>
        void Enqueue(T job);

        /// <summary>
        /// Gets a value that indicates whether the queue is empty.
        /// </summary>
        /// <returns>true if the queue is empty; otherwise, false.</returns>
        bool IsEmpty();
    }
}