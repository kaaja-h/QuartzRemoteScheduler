using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Quartz;
using QuartzRemoteScheduler.Common;
using QuartzRemoteScheduler.Common.Model;

namespace QuartzRemoteScheduler.Server.Listeners
{
    internal class EventTriggerListener:EventListenerBase<IRemoteTriggerListener>, ITriggerListener
    {



        public async Task TriggerFired(ITrigger trigger, IJobExecutionContext context,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var tr = new SerializableTrigger(trigger);
            var c = new SerializableJobExecutionContext(context);
            await RunActionOnListenersAsync(l => l.TriggerFired(tr, c, cancellationToken));
        }

        public async Task<bool> VetoJobExecution(ITrigger trigger, IJobExecutionContext context,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var tr = new SerializableTrigger(trigger);
            var c = new SerializableJobExecutionContext(context);
            var res = await RunActionOnListenersAsync(l => l.VetoJobExecution(tr, c, cancellationToken));
            return res.Any(d => d);
        }

        public async Task TriggerMisfired(ITrigger trigger, CancellationToken cancellationToken = new CancellationToken())
        {
            var tr = new SerializableTrigger(trigger);
            await RunActionOnListenersAsync(l => l.TriggerMisfired(tr, cancellationToken));
        }

        public async Task TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var tr = new SerializableTrigger(trigger);
            var c = new SerializableJobExecutionContext(context);
            await RunActionOnListenersAsync(l => l.TriggerComplete(tr, c, (int) triggerInstructionCode, cancellationToken));
        }

        public string Name { get; } = "EventTriggerListener";
    }
}