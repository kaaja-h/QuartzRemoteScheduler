using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Quartz;
using Quartz.Core;
using QuartzRemoteScheduler.Client.Model;
using QuartzRemoteScheduler.Common;
using QuartzRemoteScheduler.Common.Model;

namespace QuartzRemoteScheduler.Client.Listeners
{
    class RemoteTriggerListener:IRemoteTriggerListener
    {

        public IScheduler Scheduler { get; set; }
        private readonly IListenerManager _listenerManager;

        private async Task RunAll(Func<ITriggerListener, Task> action, TriggerKey key)
        {
            var tasks = _listenerManager.GetTriggerListeners()
                .Where(l =>
                    _listenerManager.GetTriggerListenerMatchers(l.Name).All(t => t.IsMatch(key))
                ).Select(action);
            await Task.WhenAll(tasks);
        }
        
        private async Task<IEnumerable<TRes>> RunAll<TRes>(Func<ITriggerListener, Task<TRes>> action, TriggerKey key)
        {
            var tasks = _listenerManager.GetTriggerListeners()
                .Where(l =>
                    _listenerManager.GetTriggerListenerMatchers(l.Name).All(t => t.IsMatch(key))
                ).Select(action);
            return await Task.WhenAll(tasks);
        }
        
        public RemoteTriggerListener(IListenerManager listenerManager)
        {
            _listenerManager = listenerManager;
        }

        public async Task TriggerFired(SerializableTrigger trigger, SerializableJobExecutionContext context,
            CancellationToken cancellationToken)
        {
            ITrigger t = trigger.Trigger;
            RemoteJobExecutionContext c = new RemoteJobExecutionContext(context, Scheduler);
            await RunAll(l => l.TriggerFired(t, c, cancellationToken), t.Key);
        }

        public async Task<bool> VetoJobExecution(SerializableTrigger trigger, SerializableJobExecutionContext context,
            CancellationToken cancellationToken)
        {
            ITrigger t = trigger.Trigger;
            RemoteJobExecutionContext c = new RemoteJobExecutionContext(context, Scheduler);
            var res = await RunAll(l => l.VetoJobExecution(t, c, cancellationToken), t.Key);
            return res.Any(d => d);
        }

        public async Task TriggerMisfired(SerializableTrigger trigger, CancellationToken cancellationToken)
        {
            ITrigger t = trigger.Trigger;
            await RunAll(l => l.TriggerMisfired(t, cancellationToken), t.Key);
        }

        public async Task TriggerComplete(SerializableTrigger trigger, SerializableJobExecutionContext context, int triggerInstructionCode,
            CancellationToken cancellationToken)
        {
            ITrigger t = trigger.Trigger;
            RemoteJobExecutionContext c = new RemoteJobExecutionContext(context, Scheduler);
            await RunAll(l => l.TriggerComplete(t, c, (SchedulerInstruction) triggerInstructionCode,cancellationToken), t.Key);
        }
    }
}