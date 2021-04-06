using System.Threading;
using System.Threading.Tasks;
using QuartzRemoteScheduler.Common.Model;

namespace QuartzRemoteScheduler.Common
{
    interface IRemoteTriggerListener
    {
        Task TriggerFired(SerializableTrigger trigger, SerializableJobExecutionContext context,
            CancellationToken cancellationToken);

        Task<bool> VetoJobExecution(SerializableTrigger trigger, SerializableJobExecutionContext context,
            CancellationToken cancellationToken);

        Task TriggerMisfired(SerializableTrigger trigger, CancellationToken cancellationToken);

        Task TriggerComplete(SerializableTrigger trigger, SerializableJobExecutionContext context,
            int triggerInstructionCode,
            CancellationToken cancellationToken);

    }
}