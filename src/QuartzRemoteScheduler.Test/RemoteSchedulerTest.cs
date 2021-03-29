using System;
using System.Collections.Generic;
using System.Linq;
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
            await _schedulerFixture.LocalScheduler.AddJob(detail, true);
            await _schedulerFixture.LocalScheduler.ScheduleJob(trigger);
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
            await _schedulerFixture.LocalScheduler.AddJob(detail, true);
            await _schedulerFixture.LocalScheduler.ScheduleJob(trigger);
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

        [Fact]
        public async Task GetPausedTriggerGroupsTaskAsync()
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
            var remote = await remoteScheduler.GetPausedTriggerGroups();
            var local = await _schedulerFixture.LocalScheduler.GetPausedTriggerGroups();
            Assert.Equal(remote,local);
            
        }

        [Fact]
        public async Task UnscheduleJobsTestAsync()
        {
            var group = Guid.NewGuid().ToString();
            var detail = JobBuilder.Create<TestJob>().StoreDurably(true)
                .WithDescription("dsdfsdf")
                .WithIdentity("dddd", group)
                .Build();
            var trigger1 = TriggerBuilder.Create().WithSimpleSchedule(
                    s => s.WithRepeatCount(1).WithIntervalInMinutes(10)
                ).StartAt(DateTimeOffset.Now + TimeSpan.FromMinutes(10))
                .WithIdentity("dddd", group)
                .ForJob(detail)
                .Build();
            await _schedulerFixture.LocalScheduler.AddJob(detail, true);
            await _schedulerFixture.LocalScheduler.ScheduleJob(trigger1);
            var trigger2 = TriggerBuilder.Create().WithSimpleSchedule(
                    s => s.WithRepeatCount(1).WithIntervalInMinutes(10)
                ).StartAt(DateTimeOffset.Now + TimeSpan.FromMinutes(10))
                .WithIdentity("lll", group)
                .ForJob(detail)
                .Build();
            await _schedulerFixture.LocalScheduler.ScheduleJob(trigger2);
            var remoteScheduler = await _schedulerFixture.GetRemoteSchedulerAsync();
            await remoteScheduler.UnscheduleJobs(new[] {trigger1.Key, trigger2.Key});
            var t1 = await _schedulerFixture.LocalScheduler.GetTrigger(trigger1.Key);
            var t2 = await _schedulerFixture.LocalScheduler.GetTrigger(trigger2.Key);
            Assert.Null(t1);
            Assert.Null(t2);
        }

        [Fact]
        public async Task DeleteJobTestAsync()
        {
            var detail = JobBuilder.Create<TestJob>().StoreDurably(true)
                .Build();
            var trigger1 = TriggerBuilder.Create().WithSimpleSchedule(
                    s => s.WithRepeatCount(1).WithIntervalInMinutes(10)
                ).StartAt(DateTimeOffset.Now + TimeSpan.FromMinutes(10))
                .ForJob(detail)
                .Build();
            await _schedulerFixture.LocalScheduler.AddJob(detail, true);
            await _schedulerFixture.LocalScheduler.ScheduleJob(trigger1);
            
            var remoteScheduler = await _schedulerFixture.GetRemoteSchedulerAsync();
            var j = await _schedulerFixture.LocalScheduler.GetJobDetail(detail.Key);
            Assert.NotNull(j);
            var res =await remoteScheduler.DeleteJob(j.Key);
            Assert.True(res);
            j = await _schedulerFixture.LocalScheduler.GetJobDetail(detail.Key);
            Assert.Null(j);
        }

        [Fact]
        public async Task DeleteJobsTestAsync()
        {
            var detail1 = JobBuilder.Create<TestJob>().StoreDurably(true)
                .Build();
            var trigger1 = TriggerBuilder.Create().WithSimpleSchedule(
                    s => s.WithRepeatCount(1).WithIntervalInMinutes(10)
                ).StartAt(DateTimeOffset.Now + TimeSpan.FromMinutes(10))
                .ForJob(detail1)
                .Build();
            await _schedulerFixture.LocalScheduler.AddJob(detail1, true);
            await _schedulerFixture.LocalScheduler.ScheduleJob(trigger1);
            var detail2 = JobBuilder.Create<TestJob>().StoreDurably(true)
                .Build();
            var trigger2 = TriggerBuilder.Create().WithSimpleSchedule(
                    s => s.WithRepeatCount(1).WithIntervalInMinutes(10)
                ).StartAt(DateTimeOffset.Now + TimeSpan.FromMinutes(10))
                .ForJob(detail1)
                .Build();
            await _schedulerFixture.LocalScheduler.AddJob(detail2, true);
            await _schedulerFixture.LocalScheduler.ScheduleJob(trigger2);
            var remoteScheduler = await _schedulerFixture.GetRemoteSchedulerAsync();
            var jj1 = await _schedulerFixture.LocalScheduler.GetJobDetail(detail1.Key);
            Assert.NotNull(jj1);
            var res = await remoteScheduler.DeleteJobs(new[] {detail1.Key, detail2.Key});
            //Assert.True(res);
            var j1 = await _schedulerFixture.LocalScheduler.GetJobDetail(detail1.Key);
            Assert.Null(j1);
            var j2 = await _schedulerFixture.LocalScheduler.GetJobDetail(detail2.Key);
            Assert.Null(j2);
        }

        [Fact]
        public async Task TriggerJobTestAsync()
        {
            var detail1 = JobBuilder.Create<TestJob>().StoreDurably(true)
                .Build();
            await _schedulerFixture.LocalScheduler.AddJob(detail1, true);
            var remoteScheduler = await _schedulerFixture.GetRemoteSchedulerAsync();
            await remoteScheduler.TriggerJob(detail1.Key);
            var tr = await _schedulerFixture.LocalScheduler.GetTriggersOfJob(detail1.Key);
            Assert.NotEmpty(tr);

        }
        
        [Fact]
        public async Task TriggerJobWithdataTestAsync()
        {
            var detail1 = JobBuilder.Create<TestJob>().StoreDurably(true)
                .Build();
            IDictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary["aaa"] = "aaa";
            
            await _schedulerFixture.LocalScheduler.AddJob(detail1, true);
            var remoteScheduler = await _schedulerFixture.GetRemoteSchedulerAsync();
            await remoteScheduler.TriggerJob(detail1.Key,new JobDataMap(dictionary));
            var tr = await _schedulerFixture.LocalScheduler.GetTriggersOfJob(detail1.Key);
            Assert.NotEmpty(tr);
            var map = tr.First().JobDataMap;
            Assert.NotNull(map);
            Assert.Contains(map, d => d.Key == "aaa");
            Assert.Equal(dictionary["aaa"],map["aaa"]);


        }
        
        
        [Fact]
        public async Task PauseJobTestAsync()
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
            var remoteScheduler = await _schedulerFixture.GetRemoteSchedulerAsync();
            await remoteScheduler.PauseJob(detail.Key);
            
            var state = await _schedulerFixture.LocalScheduler.GetTriggerState(trigger.Key);
            Assert.Equal(TriggerState.Paused, state);
        }
        
        [Fact]
        public async Task PauseJobsTestAsync()
        {
            string group = Guid.NewGuid().ToString();
            var detail = JobBuilder.Create<TestJob>().StoreDurably(true)
                .WithIdentity(group,group)
                .Build();
            var trigger = TriggerBuilder.Create().WithSimpleSchedule(
                    s => s.WithRepeatCount(1).WithIntervalInMinutes(10)
                ).StartAt(DateTimeOffset.Now + TimeSpan.FromMinutes(10))
                .ForJob(detail)
                .Build();
            await _schedulerFixture.LocalScheduler.AddJob(detail, true);
            await _schedulerFixture.LocalScheduler.ScheduleJob(trigger);
            var remoteScheduler = await _schedulerFixture.GetRemoteSchedulerAsync();
            await remoteScheduler.PauseJobs(GroupMatcher<JobKey>.GroupEquals(group));
            
            var state = await _schedulerFixture.LocalScheduler.GetTriggerState(trigger.Key);
            Assert.Equal(TriggerState.Paused, state);
        }
        
        [Fact]
        public async Task PauseJobs2TestAsync()
        {
            string group = Guid.NewGuid().ToString();
            var detail = JobBuilder.Create<TestJob>().StoreDurably(true)
                .WithIdentity(group,group)
                .Build();
            var trigger = TriggerBuilder.Create().WithSimpleSchedule(
                    s => s.WithRepeatCount(1).WithIntervalInMinutes(10)
                ).StartAt(DateTimeOffset.Now + TimeSpan.FromMinutes(10))
                .ForJob(detail)
                .Build();
            await _schedulerFixture.LocalScheduler.AddJob(detail, true);
            await _schedulerFixture.LocalScheduler.ScheduleJob(trigger);
            var remoteScheduler = await _schedulerFixture.GetRemoteSchedulerAsync();
            await remoteScheduler.PauseJobs(GroupMatcher<JobKey>.GroupStartsWith(group.Substring(0,group.Length-1)));
            
            var state = await _schedulerFixture.LocalScheduler.GetTriggerState(trigger.Key);
            Assert.Equal(TriggerState.Paused, state);
        }
        
        [Fact]
        public async Task PauseJobs3TestAsync()
        {
            string group = Guid.NewGuid().ToString();
            var detail = JobBuilder.Create<TestJob>().StoreDurably(true)
                .WithIdentity(group,group)
                .Build();
            var trigger = TriggerBuilder.Create().WithSimpleSchedule(
                    s => s.WithRepeatCount(1).WithIntervalInMinutes(10)
                ).StartAt(DateTimeOffset.Now + TimeSpan.FromMinutes(10))
                .ForJob(detail)
                .Build();
            await _schedulerFixture.LocalScheduler.AddJob(detail, true);
            await _schedulerFixture.LocalScheduler.ScheduleJob(trigger);
            var remoteScheduler = await _schedulerFixture.GetRemoteSchedulerAsync();
            await remoteScheduler.PauseJobs(GroupMatcher<JobKey>.GroupEndsWith(group.Substring(1)));
            
            var state = await _schedulerFixture.LocalScheduler.GetTriggerState(trigger.Key);
            Assert.Equal(TriggerState.Paused, state);
        }
        
        [Fact]
        public async Task PauseJobs4TestAsync()
        {
            string group = Guid.NewGuid().ToString();
            var detail = JobBuilder.Create<TestJob>().StoreDurably(true)
                .WithIdentity(group,group)
                .Build();
            var trigger = TriggerBuilder.Create().WithSimpleSchedule(
                    s => s.WithRepeatCount(1).WithIntervalInMinutes(10)
                ).StartAt(DateTimeOffset.Now + TimeSpan.FromMinutes(10))
                .ForJob(detail)
                .Build();
            await _schedulerFixture.LocalScheduler.AddJob(detail, true);
            await _schedulerFixture.LocalScheduler.ScheduleJob(trigger);
            var remoteScheduler = await _schedulerFixture.GetRemoteSchedulerAsync();
            await remoteScheduler.PauseJobs(GroupMatcher<JobKey>.GroupContains(group.Substring(1, group.Length-2)));
            
            var state = await _schedulerFixture.LocalScheduler.GetTriggerState(trigger.Key);
            Assert.Equal(TriggerState.Paused, state);
        }
        
        [Fact]
        public async Task PauseTriggerTestAsync()
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
            var remoteScheduler = await _schedulerFixture.GetRemoteSchedulerAsync();
            await remoteScheduler.PauseTrigger(trigger.Key);
            
            var state = await _schedulerFixture.LocalScheduler.GetTriggerState(trigger.Key);
            Assert.Equal(TriggerState.Paused, state);
        }

        [Fact]
        public async Task ResumeJobTestAsync()
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
            var remoteScheduler = await _schedulerFixture.GetRemoteSchedulerAsync();
            await _schedulerFixture.LocalScheduler.PauseJob(detail.Key);
            var state = await _schedulerFixture.LocalScheduler.GetTriggerState(trigger.Key);
            Assert.Equal(TriggerState.Paused, state);
            await remoteScheduler.ResumeJob(detail.Key);
            state = await _schedulerFixture.LocalScheduler.GetTriggerState(trigger.Key);
            Assert.Equal(TriggerState.Normal, state);
        }
        
        
        [Fact]
        public async Task ResumeTriggerTestAsync()
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
            var remoteScheduler = await _schedulerFixture.GetRemoteSchedulerAsync();
            await _schedulerFixture.LocalScheduler.PauseTrigger(trigger.Key);
            
            var state = await _schedulerFixture.LocalScheduler.GetTriggerState(trigger.Key);
            Assert.Equal(TriggerState.Paused, state);
            await remoteScheduler.ResumeTrigger(trigger.Key);
            state = await _schedulerFixture.LocalScheduler.GetTriggerState(trigger.Key);
            Assert.Equal(TriggerState.Normal, state);
        }
        
         
        [Fact]
        public async Task GetTriggerStateTestAsync()
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
            var remoteScheduler = await _schedulerFixture.GetRemoteSchedulerAsync();
            await _schedulerFixture.LocalScheduler.PauseTrigger(trigger.Key);
            
            var state = await remoteScheduler.GetTriggerState(trigger.Key);
            Assert.Equal(TriggerState.Paused, state);
            await _schedulerFixture.LocalScheduler.ResumeTrigger(trigger.Key);
            state = await remoteScheduler.GetTriggerState(trigger.Key);
            Assert.Equal(TriggerState.Normal, state);
        }

        [Fact]
        public async Task GetTriggerTestAsync()
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
            var remoteScheduler = await _schedulerFixture.GetRemoteSchedulerAsync();
            var tr = await remoteScheduler.GetTrigger(trigger.Key);
            Assert.Equal(trigger.Key.Name,tr.Key.Name);
            Assert.Equal(trigger.Key.Group,tr.Key.Group);
            Assert.Equal(trigger.Priority,tr.Priority);
            Assert.Equal(trigger.CalendarName,tr.CalendarName);
            Assert.Equal(trigger.JobKey.Name,tr.JobKey.Name);
            Assert.Equal(trigger.JobKey.Group,tr.JobKey.Group);
            Assert.Equal(trigger.MisfireInstruction,tr.MisfireInstruction);
            Assert.Equal(trigger.HasMillisecondPrecision,tr.HasMillisecondPrecision);
            Assert.Equal(trigger.StartTimeUtc,tr.StartTimeUtc);
            Assert.Equal(trigger.FinalFireTimeUtc,tr.FinalFireTimeUtc);
            
        }

        [Fact]
        public async Task ScheduleJobTestAsync()
        {
            var detail = JobBuilder.Create<TestJob>().StoreDurably(true)
                .WithDescription("dfsdf")
                .Build();
            var trigger = TriggerBuilder.Create().WithSimpleSchedule(
                    s => s.WithRepeatCount(1).WithIntervalInMinutes(10)
                ).StartAt(DateTimeOffset.Now + TimeSpan.FromMinutes(10))
                .ForJob(detail)
                .WithDescription("dfsdf")
                .Build();
            var remoteScheduler = await _schedulerFixture.GetRemoteSchedulerAsync();
            await remoteScheduler.ScheduleJob(detail, trigger);
            var createdJob = await _schedulerFixture.LocalScheduler.GetJobDetail(detail.Key);
            var createdTrigger = await _schedulerFixture.LocalScheduler.GetTrigger(trigger.Key);
            Assert.NotNull(createdJob);
            Assert.NotNull(createdTrigger);
            Assert.Equal(createdJob.Key, detail.Key);
            Assert.Equal(createdTrigger.Key, createdTrigger.Key);
            Assert.Equal(createdTrigger.Description, createdTrigger.Description);
            Assert.Equal(createdJob.Description, createdJob.Description);
            
        }

        [Fact]
        public async Task ScheduleJobTriggerTestAsync()
        {
            var detail = JobBuilder.Create<TestJob>().StoreDurably(true)
                .WithDescription("dfsdf")
                .Build();
            var trigger = TriggerBuilder.Create().WithSimpleSchedule(
                    s => s.WithRepeatCount(1).WithIntervalInMinutes(10)
                ).StartAt(DateTimeOffset.Now + TimeSpan.FromMinutes(10))
                .ForJob(detail)
                .WithDescription("dfsdf")
                .Build();
            var remoteScheduler = await _schedulerFixture.GetRemoteSchedulerAsync();
            await _schedulerFixture.LocalScheduler.AddJob(detail, true);
            await remoteScheduler.ScheduleJob(trigger);
            var createdTrigger = await _schedulerFixture.LocalScheduler.GetTrigger(trigger.Key);
            Assert.NotNull(createdTrigger);
            Assert.Equal(createdTrigger.Key, createdTrigger.Key);
            Assert.Equal(createdTrigger.Description, createdTrigger.Description);
        }
        
        [Fact]
        public async Task ScheduleJobsTestAsync()
        {
            var detail1 = JobBuilder.Create<TestJob>().StoreDurably(true)
                .WithDescription("dfsdf")
                .Build();
            var detail2 = JobBuilder.Create<TestJob>().StoreDurably(true)
                .WithDescription("dfsdf")
                .Build();
            var trigger1 = TriggerBuilder.Create().WithSimpleSchedule(
                    s => s.WithRepeatCount(1).WithIntervalInMinutes(10)
                ).StartAt(DateTimeOffset.Now + TimeSpan.FromMinutes(10))
                .ForJob(detail1)
                .WithDescription("dfsdf")
                .Build();
            var trigger2 = TriggerBuilder.Create().WithSimpleSchedule(
                    s => s.WithRepeatCount(1).WithIntervalInMinutes(10)
                ).StartAt(DateTimeOffset.Now + TimeSpan.FromMinutes(10))
                .ForJob(detail1)
                .WithDescription("dfsdf")
                .Build();
            var trigger3 = TriggerBuilder.Create().WithSimpleSchedule(
                    s => s.WithRepeatCount(1).WithIntervalInMinutes(10)
                ).StartAt(DateTimeOffset.Now + TimeSpan.FromMinutes(10))
                .ForJob(detail2)
                .WithDescription("dfsdf")
                .Build();
            Dictionary<IJobDetail, IReadOnlyCollection<ITrigger>> data =
                new Dictionary<IJobDetail, IReadOnlyCollection<ITrigger>>();
            data[detail1] = new[] {trigger1, trigger2};
            data[detail2] = new[] {trigger3};
                
            
            var remoteScheduler = await _schedulerFixture.GetRemoteSchedulerAsync();
            await remoteScheduler.ScheduleJobs(data,true);
            var createdJob1 = await _schedulerFixture.LocalScheduler.GetJobDetail(detail1.Key);
            var createdJob2 = await _schedulerFixture.LocalScheduler.GetJobDetail(detail2.Key);
            var createdTrigger1 = await _schedulerFixture.LocalScheduler.GetTrigger(trigger1.Key);
            var createdTrigger2 = await _schedulerFixture.LocalScheduler.GetTrigger(trigger2.Key);
            var createdTrigger3 = await _schedulerFixture.LocalScheduler.GetTrigger(trigger3.Key);
            Assert.NotNull(createdJob1);
            Assert.NotNull(createdJob2);
            Assert.NotNull(createdTrigger1);
            Assert.NotNull(createdTrigger2);
            Assert.NotNull(createdTrigger3);
            Assert.Equal(createdJob2.Key, detail2.Key);
            Assert.Equal(createdJob1.Key, detail1.Key);
            Assert.Equal(createdTrigger1.Key, createdTrigger1.Key);
            Assert.Equal(createdTrigger2.Key, createdTrigger2.Key);
            Assert.Equal(createdTrigger3.Key, createdTrigger3.Key);
            Assert.Equal(createdTrigger1.Description, createdTrigger1.Description);
            Assert.Equal(createdTrigger2.Description, createdTrigger2.Description);
            Assert.Equal(createdTrigger3.Description, createdTrigger3.Description);
            Assert.Equal(createdJob1.Description, createdJob1.Description);
            Assert.Equal(createdJob2.Description, createdJob2.Description);
            
        }

        [Fact]
        public async Task ScheduleJobTriggersTestAsync()
        {
            var detail1 = JobBuilder.Create<TestJob>().StoreDurably(true)
                .WithDescription("dfsdf")
                .Build();
            var trigger1 = TriggerBuilder.Create().WithSimpleSchedule(
                    s => s.WithRepeatCount(1).WithIntervalInMinutes(10)
                ).StartAt(DateTimeOffset.Now + TimeSpan.FromMinutes(10))
                .ForJob(detail1)
                .WithDescription("dfsdf")
                .Build();
            var trigger2 = TriggerBuilder.Create().WithSimpleSchedule(
                    s => s.WithRepeatCount(1).WithIntervalInMinutes(10)
                ).StartAt(DateTimeOffset.Now + TimeSpan.FromMinutes(10))
                .ForJob(detail1)
                .WithDescription("dfsdf")
                .Build();
            
            var remoteScheduler = await _schedulerFixture.GetRemoteSchedulerAsync();
            await remoteScheduler.ScheduleJob(detail1, new []{trigger1, trigger2},true);
            var createdJob1 = await _schedulerFixture.LocalScheduler.GetJobDetail(detail1.Key);
            var createdTrigger1 = await _schedulerFixture.LocalScheduler.GetTrigger(trigger1.Key);
            var createdTrigger2 = await _schedulerFixture.LocalScheduler.GetTrigger(trigger2.Key);
            Assert.NotNull(createdJob1);
            Assert.NotNull(createdTrigger1);
            Assert.NotNull(createdTrigger2);
            Assert.Equal(createdJob1.Key, detail1.Key);
            Assert.Equal(createdTrigger1.Key, createdTrigger1.Key);
            Assert.Equal(createdTrigger2.Key, createdTrigger2.Key);
            Assert.Equal(createdTrigger1.Description, createdTrigger1.Description);
            Assert.Equal(createdTrigger2.Description, createdTrigger2.Description);
            Assert.Equal(createdJob1.Description, createdJob1.Description);
        }

        [Fact]
        public async Task RescheduleJobTestAsync()
        {
            var detail = JobBuilder.Create<TestJob>().StoreDurably(true)
                .WithDescription("dfsdf")
                .Build();
            var trigger1 = TriggerBuilder.Create().WithSimpleSchedule(
                    s => s.WithRepeatCount(1).WithIntervalInMinutes(10)
                ).StartAt(DateTimeOffset.Now + TimeSpan.FromMinutes(10))
                .ForJob(detail)
                .WithDescription("dfsdf")
                .Build();
            var remoteScheduler = await _schedulerFixture.GetRemoteSchedulerAsync();
            var trigger2 = TriggerBuilder.Create().WithSimpleSchedule(
                    s => s.WithRepeatCount(1).WithIntervalInMinutes(10)
                ).StartAt(DateTimeOffset.Now + TimeSpan.FromMinutes(10))
                .ForJob(detail)
                .WithDescription("dfsdf")
                .Build();
            await _schedulerFixture.LocalScheduler.AddJob(detail, true);
            await _schedulerFixture.LocalScheduler.ScheduleJob(trigger1);
            await remoteScheduler.RescheduleJob(trigger1.Key, trigger2);
            var tr1 = await _schedulerFixture.LocalScheduler.GetTrigger(trigger1.Key);
            var tr2 = await _schedulerFixture.LocalScheduler.GetTrigger(trigger2.Key);
            Assert.Null(tr1);
            Assert.NotNull(tr2);
        }
        
        [Fact]
        public async Task AddJobTestAsync()
        {
            var detail = JobBuilder.Create<TestJob>().StoreDurably(true)
                .WithDescription("dsdfsdf")
                .Build();
            var remoteScheduler = await _schedulerFixture.GetRemoteSchedulerAsync();
            await remoteScheduler.AddJob(detail, true);
            var local = await _schedulerFixture.LocalScheduler.GetJobDetail(detail.Key);
            Assert.Equal(detail.Description, local.Description);
            Assert.Equal(detail.Key.Name, local.Key.Name);
            Assert.Equal(detail.Key.Group, local.Key.Group);
            Assert.Equal(detail.JobType, local.JobType);
            Assert.Equal(detail.Durable, local.Durable);
            Assert.Equal(detail.PersistJobDataAfterExecution, local.PersistJobDataAfterExecution);
            Assert.Equal(detail.ConcurrentExecutionDisallowed, local.ConcurrentExecutionDisallowed);
            Assert.Equal(detail.RequestsRecovery, local.RequestsRecovery);
        }
        
        [Fact]
        public async Task AddJobTest2Async()
        {
            var detail = JobBuilder.Create<TestJob>().StoreDurably(true)
                .WithDescription("dsdfsdf")
                .Build();
            var remoteScheduler = await _schedulerFixture.GetRemoteSchedulerAsync();
            await remoteScheduler.AddJob(detail, true,true);
            var local = await _schedulerFixture.LocalScheduler.GetJobDetail(detail.Key);
            Assert.Equal(detail.Description, local.Description);
            Assert.Equal(detail.Key.Name, local.Key.Name);
            Assert.Equal(detail.Key.Group, local.Key.Group);
            Assert.Equal(detail.JobType, local.JobType);
            Assert.Equal(detail.Durable, local.Durable);
            Assert.Equal(detail.PersistJobDataAfterExecution, local.PersistJobDataAfterExecution);
            Assert.Equal(detail.ConcurrentExecutionDisallowed, local.ConcurrentExecutionDisallowed);
            Assert.Equal(detail.RequestsRecovery, local.RequestsRecovery);
        }

        [Fact]
        public async Task GetTriggersOfJobTestAsync()
        {
            var detail = JobBuilder.Create<TestJob>().StoreDurably(true)
                .WithDescription("dfsdf")
                .Build();
            var trigger1 = TriggerBuilder.Create().WithSimpleSchedule(
                    s => s.WithRepeatCount(1).WithIntervalInMinutes(10)
                ).StartAt(DateTimeOffset.Now + TimeSpan.FromMinutes(10))
                .ForJob(detail)
                .WithDescription("dfsdf")
                .Build();
            var remoteScheduler = await _schedulerFixture.GetRemoteSchedulerAsync();
            var trigger2 = TriggerBuilder.Create().WithSimpleSchedule(
                    s => s.WithRepeatCount(1).WithIntervalInMinutes(10)
                ).StartAt(DateTimeOffset.Now + TimeSpan.FromMinutes(10))
                .ForJob(detail)
                .WithDescription("dfsdf")
                .Build();
            await _schedulerFixture.LocalScheduler.AddJob(detail, true);
            await _schedulerFixture.LocalScheduler.ScheduleJob(trigger1);
            await _schedulerFixture.LocalScheduler.ScheduleJob(trigger2);
            var trs = await remoteScheduler.GetTriggersOfJob(detail.Key);
            Assert.Contains(trs, d => d.Key.Equals(trigger1.Key));
            Assert.Contains(trs, d => d.Key.Equals(trigger2.Key));
        }


        [Fact]
        public async Task PauseTriggersTestAsync()
        {
            string group = Guid.NewGuid().ToString();
            var detail = JobBuilder.Create<TestJob>().StoreDurably(true)
                .WithDescription("dfsdf")
                .Build();
            var trigger1 = TriggerBuilder.Create().WithSimpleSchedule(
                    s => s.WithRepeatCount(1).WithIntervalInMinutes(10)
                ).StartAt(DateTimeOffset.Now + TimeSpan.FromMinutes(10))
                .ForJob(detail)
                .WithIdentity(Guid.NewGuid().ToString(),group)
                .Build();
            var trigger2 = TriggerBuilder.Create().WithSimpleSchedule(
                    s => s.WithRepeatCount(1).WithIntervalInMinutes(10)
                ).StartAt(DateTimeOffset.Now + TimeSpan.FromMinutes(10))
                .ForJob(detail)
                .WithIdentity(Guid.NewGuid().ToString(),group)
                .Build();
            await _schedulerFixture.LocalScheduler.AddJob(detail, true);
            await _schedulerFixture.LocalScheduler.ScheduleJob(trigger1);
            await _schedulerFixture.LocalScheduler.ScheduleJob(trigger2);
            var remoteScheduler = await _schedulerFixture.GetRemoteSchedulerAsync();
            await remoteScheduler.PauseTriggers(GroupMatcher<TriggerKey>.GroupEquals(group));
            var tr1 = await _schedulerFixture.LocalScheduler.GetTriggerState(trigger1.Key);
            var tr2 = await _schedulerFixture.LocalScheduler.GetTriggerState(trigger2.Key);
            Assert.Equal(TriggerState.Paused,tr2);
            Assert.Equal(TriggerState.Paused,tr1);

        }
    }
}