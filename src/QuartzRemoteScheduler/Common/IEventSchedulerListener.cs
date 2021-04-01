using System;
using System.Threading;
using System.Threading.Tasks;
using QuartzRemoteScheduler.Common.Model;

namespace QuartzRemoteScheduler.Common
{
    interface IRemoteSchedulerListener
    {
        Task JobScheduledAsync(SerializableTrigger trigger, CancellationToken cancellationToken);
        Task JobUnscheduledAsync(SerializableTriggerKey triggerKey, CancellationToken cancellationToken);
    }
}