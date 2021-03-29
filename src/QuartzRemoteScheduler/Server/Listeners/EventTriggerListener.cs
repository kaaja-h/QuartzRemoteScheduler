using System.Threading;
using System.Threading.Tasks;
using Quartz;

namespace QuartzRemoteScheduler.Server.Listeners
{
    internal class EventTriggerListener:ITriggerListener
    {
        private readonly IScheduler _scheduler;

        public EventTriggerListener(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public Task TriggerFired(ITrigger trigger, IJobExecutionContext context,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public Task<bool> VetoJobExecution(ITrigger trigger, IJobExecutionContext context,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.FromResult(false);
        }

        public Task TriggerMisfired(ITrigger trigger, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public Task TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public string Name { get; } = "EventTriggerListener";
    }
}