using System;
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
    class RemoteJobListener:IRemoteJobListener
    {
        
        public IScheduler Scheduler { get; set; }
        
        private async Task RunForAllAsync(Func<IJobListener, Task> func, JobKey key)
        {
            var tasks = _listenerManager.GetJobListeners()
                .Where(d => _listenerManager.GetJobListenerMatchers(d.Name).All(l => l.IsMatch(key))
                ).Select(func);
            await Task.WhenAll(tasks);
        }
        
        private readonly IListenerManager _listenerManager;

        public RemoteJobListener(IListenerManager listenerManager)
        {
            _listenerManager = listenerManager;
        }

        public async Task JobToBeExecutedAsync(SerializableJobExecutionContext context, CancellationToken cancellationToken)
        {
            var c = new RemoteJobExecutionContext( context, Scheduler);
            await RunForAllAsync(l => l.JobToBeExecuted(c, cancellationToken),c.JobDetail.Key);
        }

        public async Task JobExecutionVetoedAsync(SerializableJobExecutionContext context, CancellationToken cancellationToken)
        {
            var c = new RemoteJobExecutionContext( context, Scheduler);
            await RunForAllAsync(l => l.JobExecutionVetoed(c, cancellationToken),c.JobDetail.Key);
        }

        public async Task JobWasExecutedAsync(SerializableJobExecutionContext context, SerializableJobExecutionException jobException,
            CancellationToken cancellationToken)
        {
            var c = new RemoteJobExecutionContext( context, Scheduler);
            var ex = jobException.GetException();
            await RunForAllAsync(l => l.JobWasExecuted(c, ex,cancellationToken),c.JobDetail.Key);
        }
    }
}