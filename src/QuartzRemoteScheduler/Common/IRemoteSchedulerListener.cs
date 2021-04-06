using System.Threading;
using System.Threading.Tasks;
using QuartzRemoteScheduler.Common.Model;

namespace QuartzRemoteScheduler.Common
{
    interface IRemoteSchedulerListener
    {
        Task JobScheduledAsync(SerializableTrigger trigger, CancellationToken cancellationToken);
        Task JobUnscheduledAsync(SerializableTriggerKey triggerKey, CancellationToken cancellationToken);

        Task TriggerFinalizedAsync(SerializableTrigger trigger, CancellationToken cancellationToken);

        Task TriggerPausedAsync(SerializableTriggerKey triggerKey, CancellationToken cancellationToken);

        Task TriggersPausedAsync(string triggerGroup, CancellationToken cancellationToken);

        Task TriggerResumedAsync(SerializableTriggerKey triggerKey, CancellationToken cancellationToken);

        Task TriggersResumedAsync(string triggerGroup, CancellationToken cancellationToken);

        Task JobAddedAsync(SerializableJobDetail jobDetail, CancellationToken cancellationToken);

        Task JobDeletedAsync(SerializableJobKey jobKey, CancellationToken cancellationToken);
        
        Task JobPausedAsync(SerializableJobKey jobKey, CancellationToken cancellationToken);
        
        Task JobInterruptedAsync(SerializableJobKey jobKey, CancellationToken cancellationToken);

        Task JobsPausedAsync(string jobGroup, CancellationToken cancellationToken);
        
        Task JobResumedAsync(SerializableJobKey jobKey, CancellationToken cancellationToken);
        
        Task JobsResumedAsync(string jobGroup, CancellationToken cancellationToken);

        Task SchedulerErrorAsync(string msg, string cause, CancellationToken cancellationToken);

        Task SchedulerInStandbyModeAsync(CancellationToken cancellationToken);

        Task SchedulerStartedAsync(CancellationToken token);
        
        Task SchedulerStartingAsync(CancellationToken token);

        Task SchedulerShutdownAsync(CancellationToken cancellationToken);

        Task SchedulerShuttingdownAsync(CancellationToken cancellationToken);

        Task SchedulingDataClearedAsync(CancellationToken cancellationToken);
    }
}