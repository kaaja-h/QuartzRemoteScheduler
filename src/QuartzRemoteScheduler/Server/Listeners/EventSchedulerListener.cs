using System;
using System.Threading;
using System.Threading.Tasks;
using Quartz;
using QuartzRemoteScheduler.Common.Model;

namespace QuartzRemoteScheduler.Server.Listeners
{
    internal class EventSchedulerListener:ISchedulerListener
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
        
        
        public Task JobScheduled(ITrigger trigger, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public Task JobUnscheduled(TriggerKey triggerKey, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public Task TriggerFinalized(ITrigger trigger, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public Task TriggerPaused(TriggerKey triggerKey, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public Task TriggersPaused(string triggerGroup, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public Task TriggerResumed(TriggerKey triggerKey, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public Task TriggersResumed(string triggerGroup, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public Task JobAdded(IJobDetail jobDetail, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public Task JobDeleted(JobKey jobKey, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public Task JobPaused(JobKey jobKey, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public Task JobInterrupted(JobKey jobKey, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public Task JobsPaused(string jobGroup, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public Task JobResumed(JobKey jobKey, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public Task JobsResumed(string jobGroup, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public Task SchedulerError(string msg, SchedulerException cause,
            CancellationToken cancellationToken = new CancellationToken())
        {
            TriggerBasicData();
            return Task.CompletedTask;

        }

        public Task SchedulerInStandbyMode(CancellationToken cancellationToken = new CancellationToken())
        {
            TriggerBasicData();
            return Task.CompletedTask;
        }

        public  Task SchedulerStarted(CancellationToken cancellationToken = new CancellationToken())
        {
            TriggerBasicData();
            return Task.CompletedTask;

        }

        public Task SchedulerStarting(CancellationToken cancellationToken = new CancellationToken())
        {
            TriggerBasicData();
            return Task.CompletedTask;

        }

        public Task SchedulerShutdown(CancellationToken cancellationToken = new CancellationToken())
        {
            TriggerBasicData();
            return Task.CompletedTask;
        }

        public Task SchedulerShuttingdown(CancellationToken cancellationToken = new CancellationToken())
        {
            TriggerBasicData();
            return Task.CompletedTask;
        }

        public Task SchedulingDataCleared(CancellationToken cancellationToken = new CancellationToken())
        {
            TriggerBasicData();
            return Task.CompletedTask;
        }
    }
}