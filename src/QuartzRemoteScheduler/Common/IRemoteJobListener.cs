using System.Threading;
using System.Threading.Tasks;
using QuartzRemoteScheduler.Common.Model;

namespace QuartzRemoteScheduler.Common
{
    interface IRemoteJobListener
    {
        Task JobToBeExecutedAsync(SerializableJobExecutionContext context, CancellationToken cancellationToken);

        Task JobExecutionVetoedAsync(SerializableJobExecutionContext context, CancellationToken cancellationToken);

        Task JobWasExecutedAsync(SerializableJobExecutionContext context, SerializableJobExecutionException jobException,
            CancellationToken cancellationToken);
    }
}