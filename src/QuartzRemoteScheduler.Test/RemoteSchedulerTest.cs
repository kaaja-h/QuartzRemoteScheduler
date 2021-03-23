using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl.Matchers;
using QuartzRemoteScheduler.Test.Common;
using Xunit;

namespace QuartzRemoteScheduler.Test
{
    [Collection("BasicSchedulerCollection")]
    public class RemoteSchedulerTest : IClassFixture<BasicSchedulerFixture>
    {
        private readonly BasicSchedulerFixture _schedulerFixture;

        public RemoteSchedulerTest(BasicSchedulerFixture schedulerFixture)
        {
            _schedulerFixture = schedulerFixture;
        }

        [Fact]
        public async Task GetJobDetailTestAsync()
        {
            var detail = JobBuilder.Create<TestJob>().StoreDurably(true)
                .WithDescription("dsdfsdf")
                .Build();
            var remoteScheduler = await _schedulerFixture.GetRemoteSchedulerAsync();
            await _schedulerFixture.LocalScheduler.AddJob(detail, true);
            var remote = await remoteScheduler.GetJobDetail(detail.Key);
            var local = await _schedulerFixture.LocalScheduler.GetJobDetail(detail.Key);
            Assert.Equal(remote.Description, local.Description);
            Assert.Equal(remote.Key.Name, local.Key.Name);
            Assert.Equal(remote.Key.Group, local.Key.Group);
            Assert.Equal(remote.JobType, local.JobType);
            Assert.Equal(remote.Durable, local.Durable);
            Assert.Equal(remote.PersistJobDataAfterExecution, local.PersistJobDataAfterExecution);
            Assert.Equal(remote.ConcurrentExecutionDisallowed, local.ConcurrentExecutionDisallowed);
            Assert.Equal(remote.RequestsRecovery, local.RequestsRecovery);
        }

        [Fact]
        public async Task GetJobGroupNamesTest()
        {
            var detail = JobBuilder.Create<TestJob>().StoreDurably(true)
                .WithDescription("dsdfsdf")
                .WithIdentity("dddd", "test")
                .Build();
            await _schedulerFixture.LocalScheduler.AddJob(detail, true);
            var remoteScheduler = await _schedulerFixture.GetRemoteSchedulerAsync();
            var rem = await remoteScheduler.GetJobGroupNames();
            var loc = await _schedulerFixture.LocalScheduler.GetJobGroupNames();
            Assert.Equal(rem, loc);
        }


        [Fact]
        public async Task IsJobGroupPausedTestAsync()
        {
            var group = Guid.NewGuid().ToString();
            var detail = JobBuilder.Create<TestJob>().StoreDurably(true)
                .WithDescription("dsdfsdf")
                .WithIdentity("dddd", group)
                .Build();
            var trigger = TriggerBuilder.Create().WithSimpleSchedule(
                    s => s.WithRepeatCount(1).WithIntervalInMinutes(10)
                ).StartAt(DateTimeOffset.Now + TimeSpan.FromMinutes(10))
                .WithIdentity("dddd", group)
                .ForJob(detail)
                .Build();
            await _schedulerFixture.LocalScheduler.AddJob(detail, true);
            await _schedulerFixture.LocalScheduler.ScheduleJob(trigger);
            await _schedulerFixture.LocalScheduler.PauseJobs(GroupMatcher<JobKey>.GroupEquals(group));
            var remoteScheduler = await _schedulerFixture.GetRemoteSchedulerAsync();
            var loc = await _schedulerFixture.LocalScheduler.IsJobGroupPaused(group);
            var rem = await remoteScheduler.IsJobGroupPaused(group);
            Assert.Equal(loc, rem);
        }

        [Fact]
        public async Task IsTriggerGroupPausedTest()
        {
            var group = Guid.NewGuid().ToString();
            var detail = JobBuilder.Create<TestJob>().StoreDurably(true)
                .WithDescription("dsdfsdf")
                .WithIdentity("dddd", group)
                .Build();
            var trigger = TriggerBuilder.Create().WithSimpleSchedule(
                    s => s.WithRepeatCount(1).WithIntervalInMinutes(10)
                ).StartAt(DateTimeOffset.Now + TimeSpan.FromMinutes(10))
                .WithIdentity("dddd", group)
                .ForJob(detail)
                .Build();
            await _schedulerFixture.LocalScheduler.AddJob(detail, true);
            await _schedulerFixture.LocalScheduler.ScheduleJob(trigger);
            await _schedulerFixture.LocalScheduler.PauseTriggers(GroupMatcher<TriggerKey>.GroupEquals(group));
            var remoteScheduler = await _schedulerFixture.GetRemoteSchedulerAsync();
            var d = await remoteScheduler.IsTriggerGroupPaused(group);
            Assert.Equal(true, d);
        }

        [Fact]
        public async Task GetTriggerGroupNamesTestAsync()
        {
            var group = Guid.NewGuid().ToString();
            var detail = JobBuilder.Create<TestJob>().StoreDurably(true)
                .WithDescription("dsdfsdf")
                .WithIdentity("dddd", group)
                .Build();
            var trigger = TriggerBuilder.Create().WithSimpleSchedule(
                    s => s.WithRepeatCount(1).WithIntervalInMinutes(10)
                ).StartAt(DateTimeOffset.Now + TimeSpan.FromMinutes(10))
                .WithIdentity("dddd", group)
                .ForJob(detail)
                .Build();
            var remoteScheduler = await _schedulerFixture.GetRemoteSchedulerAsync();
            var loc = await _schedulerFixture.LocalScheduler.GetTriggerGroupNames();
            var rem = await remoteScheduler.GetTriggerGroupNames();
            Assert.Equal(loc, rem);
        }

        [Fact]
        public async Task UnscheduleJobTestAsync()
        {
            var group = Guid.NewGuid().ToString();
            var detail = JobBuilder.Create<TestJob>().StoreDurably(true)
                .WithDescription("dsdfsdf")
                .WithIdentity("dddd", group)
                .Build();
            var trigger = TriggerBuilder.Create().WithSimpleSchedule(
                    s => s.WithRepeatCount(1).WithIntervalInMinutes(10)
                ).StartAt(DateTimeOffset.Now + TimeSpan.FromMinutes(10))
                .WithIdentity("dddd", group)
                .ForJob(detail)
                .Build();
            var remoteScheduler = await _schedulerFixture.GetRemoteSchedulerAsync();
            await remoteScheduler.UnscheduleJob(trigger.Key);
            var tr = await _schedulerFixture.LocalScheduler.GetTrigger(trigger.Key);


            Assert.Null(tr);
        }
        
        
        [Fact]
        public async Task GetJobDetailMapTestAsync()
        {
            IDictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary["aaa"] = "aaa";
            dictionary["bbb"] = 1;
            dictionary["ccc"] = 1L;
            dictionary["ddd"] = false;
            var data = new JobDataMap(dictionary);
            var detail = JobBuilder.Create<TestJob>().StoreDurably(true)
                .SetJobData(data)
                .Build();
            var remoteScheduler = await _schedulerFixture.GetRemoteSchedulerAsync();
            await _schedulerFixture.LocalScheduler.AddJob(detail, true);
            
            
            var remote = await remoteScheduler.GetJobDetail(detail.Key);
            var jobData = remote.JobDataMap;
            Assert.Equal(jobData.Keys, dictionary.Keys);
            foreach (var key in jobData.Keys)
            {
                Assert.Equal(jobData[key],dictionary[key]);    
            }
            
            
            
        }
        
    }
}