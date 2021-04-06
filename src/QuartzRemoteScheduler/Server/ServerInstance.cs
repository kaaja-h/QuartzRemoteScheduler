using System;
using System.IO;
using System.Net.Sockets;
using Quartz;
using QuartzRemoteScheduler.Common;
using QuartzRemoteScheduler.Server.Listeners;
using StreamJsonRpc;

namespace QuartzRemoteScheduler.Server
{
    class ServerInstance: IDisposable
    {
        private readonly IScheduler _scheduler;
        private readonly EventSchedulerListener _schedulerListener;
        private readonly EventJobListener _eventJobListener;
        private readonly EventTriggerListener _eventTriggerListener;
        private JsonRpc _server;
        private SchedulerRpcServer _schedulerProxy;
        private TriggerRpcServer _triggerRpcProxy;
        private IRemoteSchedulerListener _remoteSchedulerListener;
        private IRemoteJobListener _remoteJobListener;
        private IRemoteTriggerListener _remoteTriggerListener;


        public ServerInstance(IScheduler scheduler, EventSchedulerListener schedulerListener, EventJobListener eventJobListener, EventTriggerListener eventTriggerListener)
        {
            _scheduler = scheduler;
            _schedulerListener = schedulerListener;
            _eventJobListener = eventJobListener;
            _eventTriggerListener = eventTriggerListener;
        }

        public void Connect(Stream stream, TcpClient client)
        {
            var connector = new LengthHeaderMessageHandler(stream, stream, new MessagePackFormatter());
            _server = new JsonRpc(connector);
            _schedulerProxy = new SchedulerRpcServer(client, _scheduler, _schedulerListener);
            _server.AddLocalRpcTarget(_schedulerProxy);
            _triggerRpcProxy = new TriggerRpcServer(_scheduler);
            _server.AddLocalRpcTarget(_triggerRpcProxy);
            
            _remoteSchedulerListener = _server.Attach<IRemoteSchedulerListener>();
            _schedulerListener.Subscribe(_remoteSchedulerListener);
            _remoteJobListener = _server.Attach<IRemoteJobListener>();
            _eventJobListener.Subscribe(_remoteJobListener);
            _remoteTriggerListener = _server.Attach<IRemoteTriggerListener>();
            _eventTriggerListener.Subscribe(_remoteTriggerListener);
            _server.Completion.ContinueWith(t => Dispose());
            _server.StartListening();
        }

        public void Dispose()
        {
            _schedulerProxy.Dispose();
            _schedulerListener.Unsubscribe(_remoteSchedulerListener);
            _eventJobListener.Unsubscribe(_remoteJobListener);
        }
    }
}