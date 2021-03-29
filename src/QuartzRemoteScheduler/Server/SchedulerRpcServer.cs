using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Quartz;
using QuartzRemoteScheduler.Common;
using QuartzRemoteScheduler.Common.Model;
using QuartzRemoteScheduler.Server.Listeners;

namespace QuartzRemoteScheduler.Server
{
    internal class SchedulerRpcServer : ISchedulerRpcService, IDisposable
    {
        private readonly TcpClient _client;
        private readonly IScheduler _scheduler;
        private readonly EventSchedulerListener _schedulerListener;

        internal SchedulerRpcServer(TcpClient client, IScheduler scheduler, EventSchedulerListener schedulerListener)
        {
            _client = client;
            _scheduler = scheduler;
            _schedulerListener = schedulerListener;
            _schedulerListener.BasicDataChanged += BasicDataChangedHandler;
            
        }
        
        private void BasicDataChangedHandler(object sender, SchedulerBasicData data)
        {
            BasicDataChanged?.Invoke(sender,data);
        }


        public event EventHandler<SchedulerBasicData> BasicDataChanged;
        public Task<SchedulerBasicData> GetBasicDataAsync()
        {
            return Task.FromResult(new SchedulerBasicData(_scheduler));
            
        }

        public async Task<bool> IsJobGroupPausedAsync(string groupName, CancellationToken token)
        {
            return await _scheduler.IsJobGroupPaused(groupName, token);
        }

        public async Task<bool> IsTriggerGroupPausedAsync(string groupName, CancellationToken cancellationToken)
        {
            return await _scheduler.IsTriggerGroupPaused(groupName,cancellationToken);
        }

        public async Task<string[]> GetJobGroupNamesAsync(CancellationToken cancellationToken)
        {
            return (await _scheduler.GetJobGroupNames(cancellationToken)).ToArray();
        }

        public async Task<string[]> GetTriggerGroupNamesAsync(CancellationToken cancellationToken)
        {
            return (await _scheduler.GetTriggerGroupNames(cancellationToken)).ToArray();
        }

        public async Task<string[]> GetPausedTriggerGroupsAsync(CancellationToken cancellationToken)
        {
            return (await _scheduler.GetPausedTriggerGroups(cancellationToken)).ToArray();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _scheduler.Start(cancellationToken);
        }

        public async Task StartDelayedAsync(long ticks, CancellationToken cancellationToken)
        {
            await _scheduler.StartDelayed(TimeSpan.FromTicks(ticks),cancellationToken);
        }

        public async Task StandbyAsync(CancellationToken cancellationToken)
        {
            await _scheduler.Standby(cancellationToken);
        }

        public async Task ShutdownAsync(CancellationToken cancellationToken)
        {
            await _scheduler.Shutdown(cancellationToken);
        }

        public async Task ShutdownAsync(bool waitForJobsToComplete, CancellationToken cancellationToken)
        {
            await _scheduler.Shutdown(waitForJobsToComplete,cancellationToken);
        }

        public async Task<string[]> GetCalendarNamesAsync(CancellationToken cancellationToken)
        {
            return (await _scheduler.GetCalendarNames(cancellationToken)).ToArray();
        }

        public async Task<bool> InterruptAsync(string fireInstanceId, CancellationToken cancellationToken)
        {
            return await _scheduler.Interrupt(fireInstanceId, cancellationToken);
        }

        public async Task ClearAsync(CancellationToken cancellationToken)
        {
            await _scheduler.Clear(cancellationToken);
        }

        public async Task<bool> UnscheduleJobAsync(SerializableTriggerKey serializableTriggerKey, CancellationToken cancellationToken)
        {
            return await _scheduler.UnscheduleJob(serializableTriggerKey, cancellationToken);
        }

        public async Task<bool> UnscheduleJobsAsync(SerializableTriggerKey[] triggerKeys, CancellationToken cancellationToken)
        {
            var keys = triggerKeys.Select(t=>(TriggerKey) t).ToArray();
            return await _scheduler.UnscheduleJobs(keys, cancellationToken);
        }

        public async Task<bool> DeleteJobAsync(SerializableJobKey serializableJobKey, CancellationToken cancellationToken)
        {
            return await _scheduler.DeleteJob(serializableJobKey, cancellationToken);
        }

        public async Task<bool> DeleteJobsAsync(SerializableJobKey[] jobKeys, CancellationToken cancellationToken)
        {
            var keys = jobKeys.Select(k => (JobKey)k).ToArray();
            return await _scheduler.DeleteJobs(keys, cancellationToken);
        }

        public async Task TriggerJobAsync(SerializableJobKey serializableJobKey, CancellationToken cancellationToken)
        {
            await _scheduler.TriggerJob(serializableJobKey,cancellationToken);
        }

        public async Task TriggerJobAsync(SerializableJobKey serializableJobKey, SerializableJobDataMap dataMap,
            CancellationToken cancellationToken)
        {
            await _scheduler.TriggerJob(serializableJobKey, dataMap.GetMap(), cancellationToken);
        }

        public async Task PauseJobAsync(SerializableJobKey serializableJobKey, CancellationToken cancellationToken)
        {
            await _scheduler.PauseJob(serializableJobKey, cancellationToken);
        }

        public async Task PauseTriggerAsync(SerializableTriggerKey serializableTriggerKey, CancellationToken cancellationToken)
        {
            await _scheduler.PauseTrigger(serializableTriggerKey, cancellationToken);
        }

        public async Task ResumeJobAsync(SerializableJobKey serializableJobKey, CancellationToken cancellationToken)
        {
            await _scheduler.ResumeJob(serializableJobKey, cancellationToken);
        }

        public async Task ResumeTriggerAsync(SerializableTriggerKey serializableTriggerKey, CancellationToken cancellationToken)
        {
            await _scheduler.ResumeTrigger(serializableTriggerKey, cancellationToken);
        }

        public async Task PauseAllAsync(CancellationToken cancellationToken)
        {
            await _scheduler.PauseAll(cancellationToken);
        }

        public async Task ResumeAllAsync(CancellationToken cancellationToken)
        {
            await _scheduler.ResumeAll(cancellationToken);
        }

        public async Task<int> GetTriggerStateAsync(SerializableTriggerKey serializableTriggerKey, CancellationToken cancellationToken)
        {
            var res = await _scheduler.GetTriggerState(serializableTriggerKey, cancellationToken);
            return (int) res;
        }

        public async Task<bool> DeleteCalendarAsync(string calName, CancellationToken cancellationToken)
        {
            return await _scheduler.DeleteCalendar(calName, cancellationToken);
        }

        public async Task<bool> InterruptAsync(SerializableJobKey serializableJobKey, CancellationToken cancellationToken)
        {
            return await _scheduler.Interrupt(serializableJobKey, cancellationToken);
        }

        public async Task<bool> CheckExistsAsync(SerializableJobKey serializableJobKey, CancellationToken cancellationToken)
        {
            return await _scheduler.CheckExists(serializableJobKey, cancellationToken);
        }

        public async Task<bool> CheckExistsAsync(SerializableTriggerKey serializableTriggerKey, CancellationToken cancellationToken)
        {
            return await _scheduler.CheckExists(serializableTriggerKey, cancellationToken);
        }

        public async Task<SerializableSchedulerMetaData> GetMetaDataAsync(CancellationToken cancellationToken)
        {
            var metadata = await _scheduler.GetMetaData(cancellationToken);
            return new SerializableSchedulerMetaData(metadata);
        }

        public async Task<string> GetCalendarAsync(string calName, CancellationToken cancellationToken)
        {
            return (await _scheduler.GetCalendar(calName, cancellationToken))?.GetType().AssemblyQualifiedName;
        }

        public async Task<SerializableJobDetail> GetJobDetailAsync(SerializableJobKey serializableJobKey, CancellationToken cancellationToken)
        {
            var res = await _scheduler.GetJobDetail(serializableJobKey, cancellationToken);
            return new SerializableJobDetail(res);
        }

        public async Task<SerializableTrigger> GetTriggerAsync(SerializableTriggerKey triggerKey, CancellationToken cancellationToken)
        {
            var tr = await _scheduler.GetTrigger(triggerKey, cancellationToken);
            return new SerializableTrigger(tr);
        }

        public async Task<DateTimeOffset> ScheduleJobAsync(SerializableJobDetail jobDetail, SerializableTrigger trigger, CancellationToken token)
        {
            var job = jobDetail.GetJobDetail();
            var tr = trigger.Trigger;
            return await _scheduler.ScheduleJob(job, tr, token);
        }

        public async Task<DateTimeOffset> ScheduleJobAsync(SerializableTrigger trigger, CancellationToken cancellationToken)
        {
            return await _scheduler.ScheduleJob(trigger.Trigger, cancellationToken);
        }

        public async Task ScheduleJobsAsync(JobTriggersPairs triggersAndJobs, bool replace, CancellationToken cancellationToken)
        {
            var d = triggersAndJobs.Data.ToDictionary(
                k => k.Detail.GetJobDetail(),
                k => (IReadOnlyCollection<ITrigger>) k.Triggers.Select(t => t.Trigger).ToArray()
            );
            await _scheduler.ScheduleJobs(d, replace, cancellationToken);
        }

        public async Task ScheduleJobAsync(SerializableJobDetail jobDetail, SerializableTrigger[] trigger, bool replace,CancellationToken token)
        {
            await _scheduler.ScheduleJob(jobDetail.GetJobDetail(), trigger.Select(t => t.Trigger).ToArray(),
                replace, token);
        }

        public async Task<DateTimeOffset?> RescheduleJobAsync(SerializableTriggerKey triggerKey, SerializableTrigger newTrigger,
            CancellationToken cancellationToken)
        {
            return await _scheduler.RescheduleJob(triggerKey, newTrigger.Trigger, cancellationToken);
        }

        public async Task AddJobAsync(SerializableJobDetail jobDetail, bool replace, bool? storeNonDurableWhileAwaitingScheduling,
            CancellationToken cancellationToken)
        {
            var job = jobDetail.GetJobDetail();
            if (storeNonDurableWhileAwaitingScheduling.HasValue)
                await _scheduler.AddJob(job, replace, storeNonDurableWhileAwaitingScheduling.Value, cancellationToken);
            else
            {
                await _scheduler.AddJob(job, replace, cancellationToken);
            }
        }

        public async Task<SerializableTrigger[]> GetTriggersOfJobAsync(SerializableJobKey jobKey, CancellationToken cancellationToken)
        {
            var res = await _scheduler.GetTriggersOfJob(jobKey, cancellationToken);
            return res.Select(t => new SerializableTrigger(t)).ToArray();
        }

        public async Task PauseJobsAsync(SerializableJobMatcher matcher, CancellationToken cancellationToken)
        {
            await _scheduler.PauseJobs(matcher.GetMatcher(), cancellationToken);
        }

        public async Task PauseTriggersAsync(SerializableTriggerMatcher matcher, CancellationToken cancellationToken)
        {
            await _scheduler.PauseTriggers(matcher.GetMatcher(), cancellationToken);
        }

        public async Task ResumeJobsAsync(SerializableJobMatcher matcher, CancellationToken cancellationToken)
        {
            await _scheduler.ResumeJobs(matcher.GetMatcher(), cancellationToken);
        }

        public async Task ResumeTriggersAsync(SerializableTriggerMatcher matcher, CancellationToken cancellationToken)
        {
            await _scheduler.ResumeTriggers(matcher.GetMatcher(), cancellationToken);
        }

        public async Task<SerializableJobKey[]> GetJobKeysAsync(SerializableJobMatcher matcher, CancellationToken cancellationToken)
        {
            var res = await _scheduler.GetJobKeys(matcher.GetMatcher(), cancellationToken);
            return res.Select(d => (SerializableJobKey) d).ToArray();
        }

        public async Task<SerializableTriggerKey[]> GetTriggerKeysAsync(SerializableTriggerMatcher matcher, CancellationToken cancellationToken)
        {
            var res = await _scheduler.GetTriggerKeys(matcher.GetMatcher(), cancellationToken);
            return res.Select(d => (SerializableTriggerKey) d).ToArray();
        }

        public async Task<SerializableJobExecutionContext[]> GetCurrentlyExecutingJobsAsync(CancellationToken cancellationToken)
        {
            var res = await _scheduler.GetCurrentlyExecutingJobs(cancellationToken);
            return res.Select(d => new SerializableJobExecutionContext(d)).ToArray();
        }

        public void Dispose()
        {
            _client?.Dispose();
            _schedulerListener.BasicDataChanged -= BasicDataChanged;
        }
    }
    
    
}