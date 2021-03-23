using System;
using System.Threading;
using System.Threading.Tasks;
using QuartzRemoteScheduler.Common.Model;

namespace QuartzRemoteScheduler.Common
{
    interface ISchedulerRpcService
    {
        event EventHandler<SchedulerBasicData> BasicDataChanged;

        Task<SchedulerBasicData> GetBasicDataAsync();

        Task<bool> IsJobGroupPausedAsync(string groupName, CancellationToken token);

        Task<bool> IsTriggerGroupPausedAsync(string groupName, CancellationToken cancellationToken);

        Task<string[]> GetJobGroupNamesAsync(CancellationToken cancellationToken);

        Task<string[]> GetTriggerGroupNamesAsync(CancellationToken cancellationToken);
        
        Task<string[]> GetPausedTriggerGroupsAsync(CancellationToken cancellationToken);
        
        Task StartAsync(CancellationToken cancellationToken);

        Task StartDelayedAsync(long ticks,CancellationToken cancellationToken);

        Task StandbyAsync(CancellationToken cancellationToken);

        Task ShutdownAsync(CancellationToken cancellationToken);

        Task ShutdownAsync(bool waitForJobsToComplete, CancellationToken cancellationToken);

        Task<string[]> GetCalendarNamesAsync(CancellationToken cancellationToken);

        Task<bool> InterruptAsync(string fireInstanceId, CancellationToken cancellationToken);

        Task ClearAsync(CancellationToken cancellationToken);


        Task<bool> UnscheduleJobAsync(SerializableTriggerKey serializableTriggerKey, CancellationToken cancellationToken);

        Task<bool> UnscheduleJobsAsync(SerializableTriggerKey[] triggerKeys, CancellationToken cancellationToken);

        Task<bool> DeleteJobAsync(SerializableJobKey serializableJobKey, CancellationToken cancellationToken);

        Task<bool> DeleteJobsAsync(SerializableJobKey[] jobKeys, CancellationToken cancellationToken);

        Task TriggerJobAsync(SerializableJobKey serializableJobKey, CancellationToken cancellationToken);

        Task PauseJobAsync(SerializableJobKey serializableJobKey, CancellationToken cancellationToken);

        Task PauseTriggerAsync(SerializableTriggerKey serializableTriggerKey, CancellationToken cancellationToken);

        Task ResumeJobAsync(SerializableJobKey serializableJobKey, CancellationToken cancellationToken);

        Task ResumeTriggerAsync(SerializableTriggerKey serializableTriggerKey, CancellationToken cancellationToken);

        Task PauseAllAsync(CancellationToken cancellationToken);

        Task ResumeAllAsync(CancellationToken cancellationToken);

        Task<int> GetTriggerStateAsync(SerializableTriggerKey serializableTriggerKey, CancellationToken cancellationToken);

        Task<bool> DeleteCalendarAsync(string calName, CancellationToken cancellationToken);

        Task<bool> InterruptAsync(SerializableJobKey serializableJobKey, CancellationToken cancellationToken);

        Task<bool> CheckExistsAsync(SerializableJobKey serializableJobKey, CancellationToken cancellationToken);

        Task<bool> CheckExistsAsync(SerializableTriggerKey serializableTriggerKey, CancellationToken cancellationToken);

        Task<SerializableSchedulerMetaData> GetMetaDataAsync(CancellationToken cancellationToken);

        Task<string> GetCalendarAsync(string calName, CancellationToken cancellationToken);

        Task<JobDetailData> GetJobDetailAsync(SerializableJobKey serializableJobKey, CancellationToken cancellationToken);
    }
}
