using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using QuartzRemoteScheduler.Test.Common;
using Xunit;

namespace QuartzRemoteScheduler.Test
{
    public class StartStopTest
    {
        [Fact]
        public async Task StartStopTestAsync()
        {
            BasicSchedulerFixture scheduler = new BasicSchedulerFixture();
            int i = 0;
            while (i < 10 && !scheduler.LocalScheduler.IsStarted)
            {
                await Task.Delay(1000);
                i++;
            }

            Assert.True(scheduler.LocalScheduler.IsStarted);

            var rem = await scheduler.GetRemoteSchedulerAsync();
            await rem.Standby();
            Assert.True(scheduler.LocalScheduler.InStandbyMode);
            await rem.Start();
            Assert.False(scheduler.LocalScheduler.InStandbyMode);
            Assert.True(scheduler.LocalScheduler.IsStarted);
            await rem.Standby();
            Assert.True(scheduler.LocalScheduler.InStandbyMode);
            await rem.StartDelayed(TimeSpan.FromSeconds(1));
            await Task.Delay(TimeSpan.FromSeconds(2));
            Assert.False(scheduler.LocalScheduler.InStandbyMode);
            Assert.True(scheduler.LocalScheduler.IsStarted);
            await rem.Shutdown();
            Assert.True(scheduler.LocalScheduler.IsShutdown);
        }

        [Fact]
        public async Task ShutdownTestAsync()
        {
            BasicSchedulerFixture scheduler = new BasicSchedulerFixture();
            int i = 0;
            while (i < 10 && !scheduler.LocalScheduler.IsStarted)
            {
                await Task.Delay(1000);
                i++;
            }
            Assert.True(scheduler.LocalScheduler.IsStarted);
            var rem = await scheduler.GetRemoteSchedulerAsync();
            await rem.Shutdown(false);
            Assert.True(scheduler.LocalScheduler.IsShutdown);
        }
    }
}