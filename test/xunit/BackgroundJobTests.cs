#region ENBREA - Copyright (C) 2023 STÜBER SYSTEMS GmbH
/*    
 *    ENBREA 
 *    
 *    Copyright (C) 2023 STÜBER SYSTEMS GmbH
 *
 *    Licensed under the MIT License, Version 2.0. 
 * 
 */
#endregion

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Enbrea.BackgroundJobs.Tests
{
    /// <summary>
    /// Unit tests for <see cref="BackgroundJobQueue<T>"/> and <see cref="BackgroundJobService<T>"/>.
    /// </summary>
    public class BackgroundJobTests
    {
        private readonly ITestOutputHelper _output;
        private readonly ServiceProvider _serviceProvider;
        private readonly IServiceCollection _services;

        public BackgroundJobTests(ITestOutputHelper output)
        {
            _output = output;
            _services = new ServiceCollection();
            _serviceProvider = _services.BuildServiceProvider();
        }

        [Fact]
        public async void EnqueueAndDequeueJobs()
        {
            var tcs1 = new TaskCompletionSource<object>();
            var tcs2 = new TaskCompletionSource<object>();
            var tcs3 = new TaskCompletionSource<object>();

            var queue = new BackgroundJobQueue<BackgroundJob>();

            queue.Enqueue(new BackgroundJob((f, t) => tcs1.Task));
            Assert.Equal(1, queue.GetCount());

            queue.Enqueue(new BackgroundJob((f, t) => tcs2.Task));
            Assert.Equal(2, queue.GetCount());

            queue.Enqueue(new BackgroundJob((f, t) => tcs3.Task));
            Assert.Equal(3, queue.GetCount());
            Assert.False(queue.IsEmpty());

            await queue.DequeueAsync(CancellationToken.None);
            Assert.Equal(2, queue.GetCount());

            await queue.DequeueAsync(CancellationToken.None);
            Assert.Equal(1, queue.GetCount());

            await queue.DequeueAsync(CancellationToken.None);
            Assert.Equal(0, queue.GetCount());
            Assert.True(queue.IsEmpty());
        }

        [Fact]
        public async void EnqueueAndExecuteJobs()
        {
            var serviceScopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();

            var tcs1 = new TaskCompletionSource<object>();
            var tcs2 = new TaskCompletionSource<object>();
            var tcs3 = new TaskCompletionSource<object>();

            var queue = new BackgroundJobQueue<BackgroundJob>();
            var service = new BackgroundJobService<BackgroundJob>(queue, serviceScopeFactory, NullLogger<BackgroundJob>.Instance);

            await service.StartAsync(CancellationToken.None);

            queue.Enqueue(new BackgroundJob((f, t) => tcs1.Task));
            queue.Enqueue(new BackgroundJob((f, t) => tcs2.Task));
            queue.Enqueue(new BackgroundJob((f, t) => tcs3.Task));

            tcs1.TrySetResult(null);
            tcs2.TrySetResult(null);
            tcs3.TrySetResult(null);

            await Task.WhenAll(tcs1.Task, tcs2.Task, tcs3.Task, Task.Delay(100));
            Assert.True(queue.IsEmpty());
        }
    }
}