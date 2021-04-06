using System.Threading;
using System.Threading.Tasks;
using Quartz;
using QuartzRemoteScheduler.Common;
using QuartzRemoteScheduler.Common.Model;

namespace QuartzRemoteScheduler.Server.Listeners
{
    internal class EventJobListener:EventListenerBase<IRemoteJobListener>,IJobListener
    {
        public async Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            SerializableJobExecutionContext c = new SerializableJobExecutionContext(context);
            await RunActionOnListenersAsync(l => l.JobToBeExecutedAsync(c, cancellationToken));
        }

        public async Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            SerializableJobExecutionContext c = new SerializableJobExecutionContext(context);
            await RunActionOnListenersAsync(l => l.JobExecutionVetoedAsync(c, cancellationToken));
        }

        public async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException,
            CancellationToken cancellationToken = new CancellationToken())
        {
            SerializableJobExecutionContext c = new SerializableJobExecutionContext(context);
            SerializableJobExecutionException ex = new SerializableJobExecutionException(jobException);
            await RunActionOnListenersAsync(l => l.JobWasExecutedAsync(c,ex, cancellationToken));
        }

        public string Name { get; } = "EventJobListener";
    }
}