using System.Threading;
using System.Threading.Tasks;
using Quartz;

namespace QuartzRemoteScheduler.Server.Listeners
{
    internal class EventJobListener:IJobListener
    {
        private readonly IScheduler _scheduler;

        public EventJobListener(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public string Name { get; } = "EventJobListener";
    }
}