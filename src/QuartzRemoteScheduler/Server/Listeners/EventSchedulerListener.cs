using System;
using System.Threading;
using System.Threading.Tasks;
using Quartz;
using QuartzRemoteScheduler.Common;
using QuartzRemoteScheduler.Common.Model;

namespace QuartzRemoteScheduler.Server.Listeners
{
    internal class EventSchedulerListener:EventListenerBase<IRemoteSchedulerListener>, ISchedulerListener
    {
        private readonly IScheduler _scheduler;
        public event EventHandler<SchedulerBasicData> BasicDataChanged;
        
        

        public EventSchedulerListener(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        

        private void TriggerBasicData()
        {
            try
            {
                if (BasicDataChanged != null)
                {
                    var data = new SchedulerBasicData(_scheduler);
                    BasicDataChanged?.Invoke(this, data);
                }
            }
            catch (Exception )
            {
                //TODO
            }
        }
        
        
        public async Task JobScheduled(ITrigger trigger, CancellationToken cancellationToken = new CancellationToken())
        {
            var serializableTrigger = new SerializableTrigger(trigger);
            await RunActionOnListenersAsync(d => d.JobScheduledAsync(serializableTrigger, cancellationToken));
        }

        public async Task JobUnscheduled(TriggerKey triggerKey, CancellationToken cancellationToken = new CancellationToken())
        {
            SerializableTriggerKey key = triggerKey; 
            await RunActionOnListenersAsync(d => d.JobUnscheduledAsync(key,cancellationToken));

        }

        public async Task TriggerFinalized(ITrigger trigger, CancellationToken cancellationToken = new CancellationToken())
        {
            var serializableTrigger = new SerializableTrigger(trigger);
            await RunActionOnListenersAsync(d => d.TriggerFinalizedAsync(serializableTrigger, cancellationToken));
        }

        public async Task TriggerPaused(TriggerKey triggerKey, CancellationToken cancellationToken = new CancellationToken())
        {
            SerializableTriggerKey key = triggerKey; 
            await RunActionOnListenersAsync(d => d.TriggerPausedAsync(key,cancellationToken));
        }

        public async Task TriggersPaused(string triggerGroup, CancellationToken cancellationToken = new CancellationToken())
        {
            await RunActionOnListenersAsync(d => d.TriggersPausedAsync(triggerGroup,cancellationToken));
        }

        public async Task TriggerResumed(TriggerKey triggerKey, CancellationToken cancellationToken = new CancellationToken())
        {
            SerializableTriggerKey key = triggerKey; 
            await RunActionOnListenersAsync(d => d.TriggerResumedAsync(key,cancellationToken));
        }

        public async Task TriggersResumed(string triggerGroup, CancellationToken cancellationToken = new CancellationToken())
        {
            await RunActionOnListenersAsync(d => d.TriggersResumedAsync(triggerGroup,cancellationToken));
        }

        public async Task JobAdded(IJobDetail jobDetail, CancellationToken cancellationToken = new CancellationToken())
        {
            var job = new SerializableJobDetail(jobDetail);
            await RunActionOnListenersAsync(d => d.JobAddedAsync(job,cancellationToken));
        }

        public async Task JobDeleted(JobKey jobKey, CancellationToken cancellationToken = new CancellationToken())
        {
            SerializableJobKey key = jobKey;
            await RunActionOnListenersAsync(d => d.JobDeletedAsync(key,cancellationToken));
        }

        public async Task JobPaused(JobKey jobKey, CancellationToken cancellationToken = new CancellationToken())
        {
            SerializableJobKey key = jobKey;
            await RunActionOnListenersAsync(d => d.JobPausedAsync(key,cancellationToken));
        }

        public async Task JobInterrupted(JobKey jobKey, CancellationToken cancellationToken = new CancellationToken())
        {
            SerializableJobKey key = jobKey;
            await RunActionOnListenersAsync(d => d.JobInterruptedAsync(key,cancellationToken));
        }

        public async Task JobsPaused(string jobGroup, CancellationToken cancellationToken = new CancellationToken())
        {
            await RunActionOnListenersAsync(d => d.JobsPausedAsync(jobGroup,cancellationToken));
        }

        public async Task JobResumed(JobKey jobKey, CancellationToken cancellationToken = new CancellationToken())
        {
            SerializableJobKey key = jobKey;
            await RunActionOnListenersAsync(d => d.JobResumedAsync(key,cancellationToken));;
        }

        public async Task JobsResumed(string jobGroup, CancellationToken cancellationToken = new CancellationToken())
        {
            await RunActionOnListenersAsync(d => d.JobsResumedAsync(jobGroup,cancellationToken));
        }

        public async Task SchedulerError(string msg, SchedulerException cause,
            CancellationToken cancellationToken = new CancellationToken())
        {
            TriggerBasicData();
            await RunActionOnListenersAsync(d => d.SchedulerErrorAsync(msg, cause.Message,cancellationToken));
        }

        public async Task SchedulerInStandbyMode(CancellationToken cancellationToken = new CancellationToken())
        {
            TriggerBasicData();
            await RunActionOnListenersAsync(d => d.SchedulerInStandbyModeAsync(cancellationToken));
        }

        public async Task SchedulerStarted(CancellationToken cancellationToken = new CancellationToken())
        {
            TriggerBasicData();
            await RunActionOnListenersAsync(d => d.SchedulerStartedAsync(cancellationToken));
        }

        public async Task SchedulerStarting(CancellationToken cancellationToken = new CancellationToken())
        {
            TriggerBasicData();
            await RunActionOnListenersAsync(d => d.SchedulerStartingAsync(cancellationToken));

        }

        public async Task SchedulerShutdown(CancellationToken cancellationToken = new CancellationToken())
        {
            TriggerBasicData();
            await RunActionOnListenersAsync(d => d.SchedulerShutdownAsync(cancellationToken));
        }

        public async Task SchedulerShuttingdown(CancellationToken cancellationToken = new CancellationToken())
        {
            TriggerBasicData();
            await RunActionOnListenersAsync(d => d.SchedulerShuttingdownAsync(cancellationToken));
        }

        public async Task SchedulingDataCleared(CancellationToken cancellationToken = new CancellationToken())
        {
            TriggerBasicData();
            await RunActionOnListenersAsync(d => d.SchedulingDataClearedAsync(cancellationToken));
        }
    }
}