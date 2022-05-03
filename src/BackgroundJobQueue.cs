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
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Enbrea.BackgroundJobs
{
    /// <summary>
    /// A thread-safe background job queue
    /// </summary>
    public class BackgroundJobQueue<T> : IBackgroundJobQueue<T>
        where T : IBackgroundJob
    {
        private readonly ConcurrentQueue<T> _jobs;
        private readonly SemaphoreSlim _signal;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundJobQueue<T>"/> class.
        /// </summary>
        public BackgroundJobQueue()
        {
            _jobs = new ConcurrentQueue<T>();
            _signal = new SemaphoreSlim(0);
        }

        /// <summary>
        /// Removes and returns the job at the beginning of the queue. 
        /// </summary>
        /// <param name="cancellationToken">The CancellationToken token to observe.</param>
        /// <returns>Dequeued task</returns>
        /// <summary>
        /// If the queue is empty this function will wait until a new job is queued or the CancellationToken is signaled.
        /// </summary>
        public async Task<IBackgroundJob> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _jobs.TryDequeue(out var job);
            return job;
        }

        /// <summary>
        /// Adds a job to the end of the queue.
        /// </summary>
        /// <param name="workItem">Pointer to work item</param>
        public void Enqueue(T job)
        {
            if (job == null)
            {
                throw new ArgumentNullException(nameof(job));
            }
            _jobs.Enqueue(job);
            _signal.Release();
        }

        /// <summary>
        /// Gets the number of jobs contained in the queue.
        /// </summary>
        /// <returns>NUmber of work items</returns>
        public int GetCount()
        {
            return _jobs.Count;
        }

        /// <summary>
        /// Gets a value that indicates whether the queue is empty.
        /// </summary>
        /// <returns>true if the queue is empty; otherwise, false.</returns>
        public bool IsEmpty() => _jobs.IsEmpty;
    }
}