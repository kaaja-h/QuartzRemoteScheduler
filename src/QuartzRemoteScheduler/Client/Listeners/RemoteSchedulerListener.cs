using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Quartz;
using Quartz.Core;
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

        public RemoteSchedulerListener(IListenerManager manager)
        {
            _manager = manager;
        }
    }

    
}