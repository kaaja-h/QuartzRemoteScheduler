using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading.Tasks;
using Quartz;
using QuartzRemoteScheduler.Client.Listeners;
using QuartzRemoteScheduler.Common;
using QuartzRemoteScheduler.Common.Configuration;
using StreamJsonRpc;

namespace QuartzRemoteScheduler.Client
{
    internal class Connector
    {
        private readonly RemoteSchedulerServerConfiguration _conf;
        
        private TcpClient _client;

        public Connector(RemoteSchedulerServerConfiguration conf)
        {
            _conf = conf;
        }
        
        public ISchedulerRpcService SchedulerRpcClient { get; private set; }
        public ITriggerRpcServer TriggerRpc { get; private set; }


        private async Task<Stream> AutorizeAsync(NetworkStream stream)
        {
            NegotiateStream authStream = new NegotiateStream(stream, false);
            try
            {
                await authStream
                    .AuthenticateAsClientAsync();
            }
            catch (Exception)
            {
                authStream.Close();
                throw;
            }

            return authStream;
        }
        
        public async Task ConnectAsync(RemoteSchedulerListener remoteSchedulerListener, RemoteJobListener remoteJobListener, RemoteTriggerListener remoteTriggerListener)
        {
            
            // Client and server use port 11000.
            var remoteEp = new IPEndPoint(_conf.Address, _conf.Port);
            _client = new TcpClient();
            _client.Connect(remoteEp);
            NetworkStream clientStream = _client.GetStream();


            var streamForConnection = (_conf.EnableNegotiateStream) ? (await AutorizeAsync(clientStream)) : clientStream;

            var connector = new LengthHeaderMessageHandler(streamForConnection, streamForConnection, new MessagePackFormatter());
            var c = new JsonRpc(connector);
            
            SchedulerRpcClient = c.Attach<ISchedulerRpcService>();
            TriggerRpc = c.Attach<ITriggerRpcServer>();
            c.AddLocalRpcTarget<IRemoteSchedulerListener>(remoteSchedulerListener, new JsonRpcTargetOptions(){DisposeOnDisconnect = true});
            c.AddLocalRpcTarget<IRemoteJobListener>( remoteJobListener, new JsonRpcTargetOptions(){DisposeOnDisconnect = true});
            c.AddLocalRpcTarget<IRemoteTriggerListener>( remoteTriggerListener, new JsonRpcTargetOptions(){DisposeOnDisconnect = true});
            c.StartListening();
            
            
            
        }
    }
}
