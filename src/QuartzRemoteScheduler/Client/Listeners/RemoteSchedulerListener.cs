using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Quartz;
using QuartzRemoteScheduler.Client.Model;
using QuartzRemoteScheduler.Common;
using QuartzRemoteScheduler.Common.Model;

namespace QuartzRemoteScheduler.Client.Listeners
{
    class RemoteSchedulerListener:IRemoteSchedulerListener
    {
        private readonly IListenerManager _manager;

        private async Task RunForAllAsync(Func<ISchedulerListener, Task> func)
        {
            var tasks = _manager.GetSchedulerListeners().Select(func);
            await Task.WhenAll(tasks);
        }

        public async Task JobScheduledAsync(SerializableTrigger trigger, CancellationToken cancellationToken)
        {
            var tr = trigger.Trigger;
            await RunForAllAsync(l=>l.JobScheduled(tr,cancellationToken));
        }

        public async Task JobUnscheduledAsync(SerializableTriggerKey triggerKey, CancellationToken cancellationToken)
        {
            TriggerKey k = triggerKey;
            await RunForAllAsync(l => l.JobUnscheduled(k, cancellationToken));
        }

        public async Task TriggerFinalizedAsync(SerializableTrigger trigger, CancellationToken cancellationToken)
        {
            var t = trigger.Trigger;
            await RunForAllAsync(l => l.TriggerFinalized(t, cancellationToken));
        }

        public async Task TriggerPausedAsync(SerializableTriggerKey triggerKey, CancellationToken cancellationToken)
        {
            TriggerKey key = triggerKey;
            await RunForAllAsync(l => l.TriggerPaused(key, cancellationToken));
        }

        public async Task TriggersPausedAsync(string triggerGroup, CancellationToken cancellationToken)
        {
            await RunForAllAsync(l => l.TriggersPaused(triggerGroup, cancellationToken));
        }

        public async Task TriggerResumedAsync(SerializableTriggerKey triggerKey, CancellationToken cancellationToken)
        {
            TriggerKey key = triggerKey;
            await RunForAllAsync(l => l.TriggerResumed(key, cancellationToken));
        }

        public async Task TriggersResumedAsync(string triggerGroup, CancellationToken cancellationToken)
        {
            await RunForAllAsync(l => l.TriggersResumed(triggerGroup, cancellationToken));
        }

        public async Task JobAddedAsync(SerializableJobDetail jobDetail, CancellationToken cancellationToken)
        {
            var job = new RemoteJobDetail(jobDetail);
            await RunForAllAsync(l => l.JobAdded(job, cancellationToken));
        }

        public async Task JobDeletedAsync(SerializableJobKey jobKey, CancellationToken cancellationToken)
        {
            JobKey key = jobKey;
            await RunForAllAsync(l => l.JobDeleted(key, cancellationToken));
        }

        public async Task JobPausedAsync(SerializableJobKey jobKey, CancellationToken cancellationToken)
        {
            JobKey key = jobKey;
            await RunForAllAsync(l => l.JobPaused(key, cancellationToken));
        }

        public async Task JobInterruptedAsync(SerializableJobKey jobKey, CancellationToken cancellationToken)
        {
            JobKey key = jobKey;
            await RunForAllAsync(l => l.JobInterrupted(key, cancellationToken));
        }

        public async Task JobsPausedAsync(string jobGroup, CancellationToken cancellationToken)
        {
            await RunForAllAsync(l => l.JobsPaused(jobGroup, cancellationToken));
        }

        public async Task JobResumedAsync(SerializableJobKey jobKey, CancellationToken cancellationToken)
        {
            JobKey key = jobKey;
            await RunForAllAsync(l => l.JobResumed(key, cancellationToken));
        }

        public async Task JobsResumedAsync(string jobGroup, CancellationToken cancellationToken)
        {
            await RunForAllAsync(l => l.JobsResumed(jobGroup, cancellationToken));
        }

        public async Task SchedulerErrorAsync(string msg, string cause, CancellationToken cancellationToken)
        {
            await RunForAllAsync(l => l.SchedulerError(msg, new SchedulerException(cause), cancellationToken));
        }

        public async Task SchedulerInStandbyModeAsync(CancellationToken cancellationToken)
        {
            await RunForAllAsync(l => l.SchedulerInStandbyMode(cancellationToken));
        }

        public async Task SchedulerStartedAsync(CancellationToken cancellationToken)
        {
            await RunForAllAsync(l => l.SchedulerStarted(cancellationToken));
        }

        public async Task SchedulerStartingAsync(CancellationToken cancellationToken)
        {
            await RunForAllAsync(l => l.SchedulerStarting(cancellationToken));
        }

        public async Task SchedulerShutdownAsync(CancellationToken cancellationToken)
        {
            await RunForAllAsync(l => l.SchedulerShutdown(cancellationToken));
        }

        public async Task SchedulerShuttingdownAsync(CancellationToken cancellationToken)
        {
            await RunForAllAsync(l => l.SchedulerShuttingdown(cancellationToken));
        }

        public async Task SchedulingDataClearedAsync(CancellationToken cancellationToken)
        {
            await RunForAllAsync(l => l.SchedulingDataCleared(cancellationToken));
        }

        public RemoteSchedulerListener(IListenerManager manager)
        {
            _manager = manager;
        }
    }

    
}