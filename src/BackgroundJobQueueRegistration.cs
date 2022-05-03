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

namespace Enbrea.BackgroundJobs
{
    /// <summary>
    /// <see cref="IServiceCollection"/> extensions for middleware registration.
    /// </summary>
    public static class BackgroundJobQueueRegistration
    {
        /// <summary>
        /// Register a background job queue.
        /// </summary>
        /// <typeparam name="T">Type of background job</typeparam>
        /// <param name="services">Service collection</param>
        public static void AddBackgroundJobQueue<T>(this IServiceCollection services)
            where T: IBackgroundJob
        {
            services.AddSingleton<BackgroundJobQueue<T>>();
            services.AddHostedService<BackgroundJobService<T>>();
        }
    }
}