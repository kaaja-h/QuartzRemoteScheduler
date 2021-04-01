using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Threading;
using Quartz;
using Quartz.Core;
using QuartzRemoteScheduler.Client.Listeners;
using QuartzRemoteScheduler.Common.Configuration;

namespace QuartzRemoteScheduler.Client
{
    /// <summary>
    /// Class for creating scheduler client for remote server
    /// </summary>
    public class RemoteSchedulerFactory : ISchedulerFactory
    {
        private readonly RemoteSchedulerServerConfiguration _conf;

        /// <summary>
        /// Constructor with configuration
        /// </summary>
        /// <param name="conf">Connection configuration</param>
        public RemoteSchedulerFactory(RemoteSchedulerServerConfiguration conf)
        {
            _conf = conf;

            _scheduler = new AsyncLazy<RemoteScheduler>(async () => await PrepareSchedulerAsync(), null);
        }

        private async Task<RemoteScheduler> PrepareSchedulerAsync()
        {
            Connector c = new Connector(_conf);
            var listenerManger = new ListenerManagerImpl();
            await c.ConnectAsync(listenerManger);
            var basicData = await c.SchedulerRpcClient.GetBasicDataAsync();
            var sch = new RemoteScheduler(basicData, c,listenerManger);
            return sch;
        }

        private AsyncLazy<RemoteScheduler> _scheduler;

        /// <summary>
        /// <see cref="ISchedulerFactory.GetAllSchedulers(System.Threading.CancellationToken)"/>
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IReadOnlyList<IScheduler>> GetAllSchedulers(
            CancellationToken cancellationToken = new CancellationToken())
        {
            var res = await _scheduler.GetValueAsync(cancellationToken);
            return new[] {res};
        }

        /// <summary>
        /// <see cref="ISchedulerFactory.GetScheduler(System.Threading.CancellationToken)"/>
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IScheduler> GetScheduler(CancellationToken cancellationToken = new CancellationToken())
        {
            return await _scheduler.GetValueAsync(cancellationToken);
        }

        /// <summary>
        /// <see cref="ISchedulerFactory.GetScheduler(string,System.Threading.CancellationToken)"/>
        /// </summary>
        /// <param name="schedName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IScheduler> GetScheduler(string schedName,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var res = await _scheduler.GetValueAsync(cancellationToken);
            if (res.SchedulerName == schedName)
                return res;
            return null;
        }
    }
}