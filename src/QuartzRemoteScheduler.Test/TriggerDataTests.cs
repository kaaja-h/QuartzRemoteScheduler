using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Quartz;
using QuartzRemoteScheduler.Test.Common;
using Xunit;

namespace QuartzRemoteScheduler.Test
{
    [Collection("BasicSchedulerCollection")]
    public class TriggerDataTests
    {
        private readonly BasicSchedulerFixture _schedulerFixture;

        public TriggerDataTests(BasicSchedulerFixture schedulerFixture)
        {
            _schedulerFixture = schedulerFixture;
        }

        private async Task<(IJobDetail detail, ITrigger trigger)> PrepareJobAndTrigger()
        {
            var detail = JobBuilder.Create<TestJob>().StoreDurably(true)
                .Build();
            var trigger = TriggerBuilder.Create().WithSimpleSchedule(
                    s => s.WithRepeatCount(1).WithIntervalInMinutes(10)
                ).StartAt(DateTimeOffset.Now + TimeSpan.FromMinutes(10))
                .ForJob(detail)
                .Build();
            await _schedulerFixture.LocalScheduler.AddJob(detail, true);
            await _schedulerFixture.LocalScheduler.ScheduleJob(trigger);
            return (detail, trigger);
        }

        

        private async Task<(T rem, T loc)> GenericTestAsync<T>(Func<ITrigger,T> extractor)
        {
            var (_, trigger) = await PrepareJobAndTrigger();
            var remoteScheduler = await _schedulerFixture.GetRemoteSchedulerAsync();
            var remoteTrigger = await remoteScheduler.GetTrigger(trigger.Key);
            var localTrigger = await _schedulerFixture.LocalScheduler.GetTrigger(trigger.Key);
            var rem = extractor(remoteTrigger);
            var loc = extractor(localTrigger);

            return (rem, loc);

        }

        [Fact]
        public async Task GetFireTimeAfterTestAsync()
        {
            var (rem, loc) = await GenericTestAsync(trigger => trigger.GetFireTimeAfter(DateTimeOffset.Now));
            Assert.Equal(loc,rem);
        }
        
        [Fact]
        public async Task GetMayFireAgainTestAsync()
        {
            var (rem, loc) = await GenericTestAsync(trigger => trigger.GetMayFireAgain());
            Assert.Equal(loc,rem);
        }
        
        [Fact]
        public async Task GetNextFireTimeUtcTestAsync()
        {
            var (rem, loc) = await GenericTestAsync(trigger => trigger.GetNextFireTimeUtc());
            Assert.Equal(loc,rem);
        }
        
        [Fact]
        public async Task GetPreviousFireTimeUtcTestAsync()
        {
            var (rem, loc) = await GenericTestAsync(trigger => trigger.GetPreviousFireTimeUtc());
            Assert.Equal(loc,rem);
        }
        
        [Fact]
        public async Task CompareTestAsync()
        {
            var (_, trigger) = await PrepareJobAndTrigger();
            var remoteScheduler = await _schedulerFixture.GetRemoteSchedulerAsync();
          
                var remoteTrigger = await remoteScheduler.GetTrigger(trigger.Key);
            

            var localTrigger = await _schedulerFixture.LocalScheduler.GetTrigger(trigger.Key);
            var res = remoteTrigger.CompareTo(localTrigger);
            Assert.Equal(0, res);

        }
        
        [Fact]
        public async Task SimpleTriggerTestAsync()
        {
            var (_, trigger) = await PrepareJobAndTrigger();
            var remoteScheduler = await _schedulerFixture.GetRemoteSchedulerAsync();
            var remoteTrigger = await remoteScheduler.GetTrigger(trigger.Key) as ISimpleTrigger;
            var localTrigger = await _schedulerFixture.LocalScheduler.GetTrigger(trigger.Key) as ISimpleTrigger;
            Assert.NotNull(remoteTrigger);
            Assert.NotNull(localTrigger);
            Assert.Equal(localTrigger.RepeatCount, remoteTrigger.RepeatCount);
            Assert.Equal(localTrigger.RepeatInterval, remoteTrigger.RepeatInterval);
            Assert.Equal(localTrigger.TimesTriggered, remoteTrigger.TimesTriggered);

        }

        [Fact]
        public async Task CronTriggerTestAsync()
        {
            var detail = JobBuilder.Create<TestJob>().StoreDurably(true)
                .Build();
            var trigger = TriggerBuilder.Create().WithCronSchedule(
                    "0 0/5 * * * ?"
                ).StartAt(DateTimeOffset.Now + TimeSpan.FromMinutes(10))
                .ForJob(detail)
                .Build();
            await _schedulerFixture.LocalScheduler.AddJob(detail, true);
            await _schedulerFixture.LocalScheduler.ScheduleJob(trigger);
            var remoteScheduler = await _schedulerFixture.GetRemoteSchedulerAsync();
            var remoteTrigger = await remoteScheduler.GetTrigger(trigger.Key) as ICronTrigger;
            var localTrigger = await _schedulerFixture.LocalScheduler.GetTrigger(trigger.Key) as ICronTrigger;
            Assert.Equal(remoteTrigger.CronExpressionString, localTrigger.CronExpressionString);
            Assert.Equal(remoteTrigger.TimeZone, localTrigger.TimeZone);
            Assert.Equal(remoteTrigger.GetExpressionSummary(), localTrigger.GetExpressionSummary());
        }
        
        [Fact]
        public async Task CronTriggerSetCronExpressionTestAsync()
        {
            var detail = JobBuilder.Create<TestJob>().StoreDurably(true)
                .Build();
            var trigger = TriggerBuilder.Create().WithCronSchedule(
                    "0 0/5 * * * ?"
                ).StartAt(DateTimeOffset.Now + TimeSpan.FromMinutes(10))
                .ForJob(detail)
                .Build();
            await _schedulerFixture.LocalScheduler.AddJob(detail, true);
            await _schedulerFixture.LocalScheduler.ScheduleJob(trigger);
            var remoteScheduler = await _schedulerFixture.GetRemoteSchedulerAsync();
            var remoteTrigger = await remoteScheduler.GetTrigger(trigger.Key) as ICronTrigger;
            var localTrigger = await _schedulerFixture.LocalScheduler.GetTrigger(trigger.Key) as ICronTrigger;
            remoteTrigger.CronExpressionString = "0 0/6 * * * ?";
            localTrigger.CronExpressionString = "0 0/6 * * * ?";
            Assert.Equal(remoteTrigger.CronExpressionString, localTrigger.CronExpressionString);
            Assert.Equal(remoteTrigger.GetExpressionSummary(), localTrigger.GetExpressionSummary());
        }
        



        


    }
}