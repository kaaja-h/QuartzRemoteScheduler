using System;
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
    class SchedulerRpcServer : ISchedulerRpcService, IDisposable
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
            return await _scheduler.UnscheduleJob(serializableTriggerKey.ToKey(), cancellationToken);
        }

        public async Task<bool> UnscheduleJobsAsync(SerializableTriggerKey[] triggerKeys, CancellationToken cancellationToken)
        {
            var keys = triggerKeys.Select(k => k.ToKey()).ToArray();
            return await _scheduler.UnscheduleJobs(keys, cancellationToken);
        }

        public async Task<bool> DeleteJobAsync(SerializableJobKey serializableJobKey, CancellationToken cancellationToken)
        {
            return await _scheduler.DeleteJob(serializableJobKey.ToKey(), cancellationToken);
        }

        public async Task<bool> DeleteJobsAsync(SerializableJobKey[] jobKeys, CancellationToken cancellationToken)
        {
            var keys = jobKeys.Select(k => k.ToKey()).ToArray();
            return await _scheduler.DeleteJobs(keys, cancellationToken);
        }

        public async Task TriggerJobAsync(SerializableJobKey serializableJobKey, CancellationToken cancellationToken)
        {
            await _scheduler.TriggerJob(serializableJobKey.ToKey(),cancellationToken);
        }

        public async Task PauseJobAsync(SerializableJobKey serializableJobKey, CancellationToken cancellationToken)
        {
            await _scheduler.PauseJob(serializableJobKey.ToKey(), cancellationToken);
        }

        public async Task PauseTriggerAsync(SerializableTriggerKey serializableTriggerKey, CancellationToken cancellationToken)
        {
            await _scheduler.PauseTrigger(serializableTriggerKey.ToKey(), cancellationToken);
        }

        public async Task ResumeJobAsync(SerializableJobKey serializableJobKey, CancellationToken cancellationToken)
        {
            await _scheduler.ResumeJob(serializableJobKey.ToKey(), cancellationToken);
        }

        public async Task ResumeTriggerAsync(SerializableTriggerKey serializableTriggerKey, CancellationToken cancellationToken)
        {
            await _scheduler.ResumeTrigger(serializableTriggerKey.ToKey(), cancellationToken);
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
            var res = await _scheduler.GetTriggerState(serializableTriggerKey.ToKey(), cancellationToken);
            return (int) res;
        }

        public async Task<bool> DeleteCalendarAsync(string calName, CancellationToken cancellationToken)
        {
            return await _scheduler.DeleteCalendar(calName, cancellationToken);
        }

        public async Task<bool> InterruptAsync(SerializableJobKey serializableJobKey, CancellationToken cancellationToken)
        {
            return await _scheduler.Interrupt(serializableJobKey.ToKey(), cancellationToken);
        }

        public async Task<bool> CheckExistsAsync(SerializableJobKey serializableJobKey, CancellationToken cancellationToken)
        {
            return await _scheduler.CheckExists(serializableJobKey.ToKey(), cancellationToken);
        }

        public async Task<bool> CheckExistsAsync(SerializableTriggerKey serializableTriggerKey, CancellationToken cancellationToken)
        {
            return await _scheduler.CheckExists(serializableTriggerKey.ToKey(), cancellationToken);
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

        public async Task<JobDetailData> GetJobDetailAsync(SerializableJobKey serializableJobKey, CancellationToken cancellationToken)
        {
            var res = await _scheduler.GetJobDetail(serializableJobKey.ToKey(), cancellationToken);
            return new JobDetailData(res);
        }

        public void Dispose()
        {
            _client?.Dispose();
            _schedulerListener.BasicDataChanged -= BasicDataChanged;
        }
    }
    
    
}