using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Quartz;
using Quartz.Core;
using Quartz.Impl.Matchers;
using Quartz.Spi;
using QuartzRemoteScheduler.Client.Model;
using QuartzRemoteScheduler.Common.Model;

namespace QuartzRemoteScheduler.Client
{
    internal class RemoteScheduler:IScheduler
    {

        private SchedulerBasicData _data;
        private Connector _connector;

        public  RemoteScheduler(SchedulerBasicData data, Connector connector)
        {
            _data = data;
            _connector = connector;
            _connector.SchedulerRpcClient.BasicDataChanged += (sender, d) => _data = d;
        }
        
        
        public async Task<bool> IsJobGroupPaused(string groupName, CancellationToken cancellationToken = new CancellationToken())
        {
            return await _connector.SchedulerRpcClient.IsJobGroupPausedAsync(groupName, cancellationToken);
        }

        public async Task<bool> IsTriggerGroupPaused(string groupName, CancellationToken cancellationToken = new CancellationToken())
        {
            return await _connector.SchedulerRpcClient.IsTriggerGroupPausedAsync(groupName, cancellationToken);
        }

        public async Task<SchedulerMetaData> GetMetaData(CancellationToken cancellationToken = new CancellationToken())
        {
            return (await _connector.SchedulerRpcClient.GetMetaDataAsync(cancellationToken)).ToData();
        }

        public async Task<IReadOnlyCollection<IJobExecutionContext>> GetCurrentlyExecutingJobs(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyCollection<string>> GetJobGroupNames(CancellationToken cancellationToken = new CancellationToken())
        {
            return await _connector.SchedulerRpcClient.GetJobGroupNamesAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<string>> GetTriggerGroupNames(CancellationToken cancellationToken = new CancellationToken())
        {
            return await _connector.SchedulerRpcClient.GetTriggerGroupNamesAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<string>> GetPausedTriggerGroups(CancellationToken cancellationToken = new CancellationToken())
        {
            return await _connector.SchedulerRpcClient.GetPausedTriggerGroupsAsync(cancellationToken);
        }

        public async Task Start(CancellationToken cancellationToken = new CancellationToken())
        {
            await _connector.SchedulerRpcClient.StartAsync(cancellationToken);
        }

        public async Task StartDelayed(TimeSpan delay, CancellationToken cancellationToken = new CancellationToken())
        {
            await _connector.SchedulerRpcClient.StartDelayedAsync(delay.Ticks, cancellationToken);
        }

        public async Task Standby(CancellationToken cancellationToken = new CancellationToken())
        {
            await _connector.SchedulerRpcClient.StandbyAsync(cancellationToken);
        }

        public async Task Shutdown(CancellationToken cancellationToken = new CancellationToken())
        {
            await _connector.SchedulerRpcClient.ShutdownAsync(cancellationToken);
        }

        public async Task Shutdown(bool waitForJobsToComplete, CancellationToken cancellationToken = new CancellationToken())
        {
            await _connector.SchedulerRpcClient.ShutdownAsync(waitForJobsToComplete,cancellationToken);
        }

        public async Task<DateTimeOffset> ScheduleJob(IJobDetail jobDetail, ITrigger trigger, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public async Task<DateTimeOffset> ScheduleJob(ITrigger trigger, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public async Task ScheduleJobs(IReadOnlyDictionary<IJobDetail, IReadOnlyCollection<ITrigger>> triggersAndJobs, bool replace,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public async Task ScheduleJob(IJobDetail jobDetail, IReadOnlyCollection<ITrigger> triggersForJob, bool replace,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UnscheduleJob(TriggerKey triggerKey, CancellationToken cancellationToken = new CancellationToken())
        {
            return await _connector.SchedulerRpcClient.UnscheduleJobAsync(new SerializableTriggerKey(triggerKey), cancellationToken);
        }

        public async Task<bool> UnscheduleJobs(IReadOnlyCollection<TriggerKey> triggerKeys, CancellationToken cancellationToken = new CancellationToken())
        {
            var keys = triggerKeys.Select(k => new SerializableTriggerKey(k)).ToArray();
            return await _connector.SchedulerRpcClient.UnscheduleJobsAsync(keys, cancellationToken);
        }

        public async Task<DateTimeOffset?> RescheduleJob(TriggerKey triggerKey, ITrigger newTrigger,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public async Task AddJob(IJobDetail jobDetail, bool replace, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public async Task AddJob(IJobDetail jobDetail, bool replace, bool storeNonDurableWhileAwaitingScheduling,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteJob(JobKey jobKey, CancellationToken cancellationToken = new CancellationToken())
        {
            return await _connector.SchedulerRpcClient.DeleteJobAsync(new SerializableJobKey(jobKey),cancellationToken);
        }

        public async Task<bool> DeleteJobs(IReadOnlyCollection<JobKey> jobKeys, CancellationToken cancellationToken = new CancellationToken())
        {
            var keys = jobKeys.Select(k => new SerializableJobKey(k)).ToArray();
            return await _connector.SchedulerRpcClient.DeleteJobsAsync(keys, cancellationToken);
        }

        public async Task TriggerJob(JobKey jobKey, CancellationToken cancellationToken = new CancellationToken())
        {
            await _connector.SchedulerRpcClient.TriggerJobAsync(new SerializableJobKey(jobKey), cancellationToken);
        }

        public async Task TriggerJob(JobKey jobKey, JobDataMap data, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public async Task PauseJob(JobKey jobKey, CancellationToken cancellationToken = new CancellationToken())
        {
            await _connector.SchedulerRpcClient.PauseJobAsync(new SerializableJobKey(jobKey), cancellationToken);
        }

        public async Task PauseJobs(GroupMatcher<JobKey> matcher, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public async Task PauseTrigger(TriggerKey triggerKey, CancellationToken cancellationToken = new CancellationToken())
        {
            await _connector.SchedulerRpcClient.PauseTriggerAsync(new SerializableTriggerKey(triggerKey),cancellationToken);
        }

        public async Task PauseTriggers(GroupMatcher<TriggerKey> matcher, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public async Task ResumeJob(JobKey jobKey, CancellationToken cancellationToken = new CancellationToken())
        {
            await _connector.SchedulerRpcClient.ResumeJobAsync(new SerializableJobKey(jobKey), cancellationToken);
        }

        public async Task ResumeJobs(GroupMatcher<JobKey> matcher, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public async Task ResumeTrigger(TriggerKey triggerKey, CancellationToken cancellationToken = new CancellationToken())
        {
            await _connector.SchedulerRpcClient.ResumeTriggerAsync(new SerializableTriggerKey(triggerKey), cancellationToken);
        }

        public async Task ResumeTriggers(GroupMatcher<TriggerKey> matcher, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public async Task PauseAll(CancellationToken cancellationToken = new CancellationToken())
        {
            await _connector.SchedulerRpcClient.PauseAllAsync(cancellationToken);
        }

        public async Task ResumeAll(CancellationToken cancellationToken = new CancellationToken())
        {
            await _connector.SchedulerRpcClient.ResumeAllAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<JobKey>> GetJobKeys(GroupMatcher<JobKey> matcher, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyCollection<ITrigger>> GetTriggersOfJob(JobKey jobKey, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyCollection<TriggerKey>> GetTriggerKeys(GroupMatcher<TriggerKey> matcher, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public async Task<IJobDetail> GetJobDetail(JobKey jobKey, CancellationToken cancellationToken = new CancellationToken())
        {
            var detail = await _connector.SchedulerRpcClient.GetJobDetailAsync(new SerializableJobKey(jobKey), cancellationToken);
            return new RemoteJobDetail(detail);
        }

        public async Task<ITrigger> GetTrigger(TriggerKey triggerKey, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public async Task<TriggerState> GetTriggerState(TriggerKey triggerKey, CancellationToken cancellationToken = new CancellationToken())
        {
            var res = await _connector.SchedulerRpcClient.GetTriggerStateAsync(new SerializableTriggerKey(triggerKey), cancellationToken);
            return (TriggerState) res;
        }

        public async Task AddCalendar(string calName, ICalendar calendar, bool replace, bool updateTriggers,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteCalendar(string calName, CancellationToken cancellationToken = new CancellationToken())
        {
            return await _connector.SchedulerRpcClient.DeleteCalendarAsync(calName, cancellationToken);
        }


        private ICalendar PrepareCalendar(string type)
        {
            try
            {
                var t = Type.GetType(type);
                return (ICalendar) Activator.CreateInstance(t);
            }
            catch (Exception)
            {
                
            }

            return null;
        }

        public async Task<ICalendar> GetCalendar(string calName, CancellationToken cancellationToken = new CancellationToken())
        {
            var t = await _connector.SchedulerRpcClient.GetCalendarAsync(calName, cancellationToken);
            return PrepareCalendar(t);
        }

        public async Task<IReadOnlyCollection<string>> GetCalendarNames(CancellationToken cancellationToken = new CancellationToken())
        {
            return await _connector.SchedulerRpcClient.GetCalendarNamesAsync(cancellationToken);
        }

        public async Task<bool> Interrupt(JobKey jobKey, CancellationToken cancellationToken = new CancellationToken())
        {
            return await _connector.SchedulerRpcClient.InterruptAsync(new SerializableJobKey(jobKey), cancellationToken);
        }

        public async Task<bool> Interrupt(string fireInstanceId, CancellationToken cancellationToken = new CancellationToken())
        {
            return await _connector.SchedulerRpcClient.InterruptAsync(fireInstanceId, cancellationToken);
        }

        public async Task<bool> CheckExists(JobKey jobKey, CancellationToken cancellationToken = new CancellationToken())
        {
            return await _connector.SchedulerRpcClient.CheckExistsAsync(new SerializableJobKey(jobKey), cancellationToken);
        }

        public async Task<bool> CheckExists(TriggerKey triggerKey, CancellationToken cancellationToken = new CancellationToken())
        {
            return await _connector.SchedulerRpcClient.CheckExistsAsync(new SerializableTriggerKey(triggerKey), cancellationToken);
        }

        public async Task Clear(CancellationToken cancellationToken = new CancellationToken())
        {
            await _connector.SchedulerRpcClient.ClearAsync(cancellationToken);
        }

        public string SchedulerName => _data.SchedulerName;
        public string SchedulerInstanceId => _data.SchedulerInstanceId;
        public SchedulerContext Context { get; } = null;
        public bool InStandbyMode => _data.InStandbyMode;
        public bool IsShutdown => _data.IsShutdown;
        public IJobFactory JobFactory { get; set; }
        public IListenerManager ListenerManager { get; } = new ListenerManagerImpl();
        public bool IsStarted => _data.IsStarted;
    }
}