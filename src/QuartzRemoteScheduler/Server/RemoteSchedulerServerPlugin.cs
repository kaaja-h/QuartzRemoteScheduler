using System.Net.Security;
using System.Threading;
using System.Threading.Tasks;
using Quartz;
using Quartz.Spi;
using QuartzRemoteScheduler.Common.Configuration;
using QuartzRemoteScheduler.Server.Listeners;

namespace QuartzRemoteScheduler.Server
{
    /// <summary>
    /// Plugin
    /// </summary>
    public class RemoteSchedulerServerPlugin :ISchedulerPlugin
    {

        /// <summary>
        /// server address
        /// </summary>
        public string Address { get; set; } = "127.0.0.1";
        
        /// <summary>
        /// server port
        /// </summary>
        public int Port { get; set; } = 10011;

        
        /// <summary>
        /// use negotiate stream <see cref="NegotiateStream"/>
        /// </summary>
        public bool EnableNegotiateStream { get; set; } = false;


        private RemoteSchedulerServerConfiguration _configuration;
        private IncomingRequestListener _listener;
        private readonly CancellationTokenSource _cancelationSource = new CancellationTokenSource();
        private IScheduler _scheduler;
        private EventSchedulerListener _schedulerListener;
        private EventJobListener _eventJobListener;
        private EventTriggerListener _eventTriggerListener;
        
        
        /// <summary>
        /// <see cref="ISchedulerPlugin.Initialize"/>
        /// </summary>
        /// <param name="pluginName"></param>
        /// <param name="scheduler"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task Initialize(string pluginName, IScheduler scheduler, CancellationToken cancellationToken = new CancellationToken())
        {
            _configuration = new RemoteSchedulerServerConfiguration(Address, Port, EnableNegotiateStream);
            _scheduler = scheduler;
            _schedulerListener = new EventSchedulerListener(_scheduler);
            _eventJobListener = new EventJobListener();
            _eventTriggerListener = new EventTriggerListener();
            _listener = new IncomingRequestListener(_configuration,_scheduler, _schedulerListener, _eventJobListener, _eventTriggerListener);
            _scheduler.ListenerManager.AddSchedulerListener(_schedulerListener);
            _scheduler.ListenerManager.AddTriggerListener(_eventTriggerListener);
            _scheduler.ListenerManager.AddJobListener(_eventJobListener);
            var t = new Task(() => _listener.Listen(_cancelationSource.Token));
            t.Start();
            return Task.CompletedTask;
        }

        /// <see cref="ISchedulerPlugin.Start"/>
        public Task Start(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        /// <see cref="ISchedulerPlugin.Shutdown"/>
        public Task Shutdown(CancellationToken cancellationToken = new CancellationToken())
        {
            _cancelationSource.Cancel();
            return Task.CompletedTask;
        }
    }
}