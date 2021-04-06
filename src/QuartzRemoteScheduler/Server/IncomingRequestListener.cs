using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using Quartz;
using QuartzRemoteScheduler.Common.Configuration;
using QuartzRemoteScheduler.Server.Listeners;

namespace QuartzRemoteScheduler.Server
{
    internal class IncomingRequestListener
    {
        private readonly RemoteSchedulerServerConfiguration _configuration;
        private readonly IScheduler _scheduler;
        private readonly EventSchedulerListener _schedulerListener;
        private readonly EventJobListener _eventJobListener;
        private readonly EventTriggerListener _eventTriggerListener;

        public IncomingRequestListener(RemoteSchedulerServerConfiguration configuration,IScheduler scheduler, EventSchedulerListener schedulerListener, EventJobListener eventJobListener, EventTriggerListener eventTriggerListener)
        {
            _configuration = configuration;
            _scheduler = scheduler;
            _schedulerListener = schedulerListener;
            _eventJobListener = eventJobListener;
            _eventTriggerListener = eventTriggerListener;
        }
        
        public void Listen(CancellationToken cancelationToken)
        {
            
            TcpListener listener = new TcpListener(_configuration.Address, +_configuration.Port);
            // Listen for incoming connections.
            listener.Start();
            cancelationToken.Register(() => listener.Stop());
            while (!cancelationToken.IsCancellationRequested)
            {
                TcpClient clientRequest;
                clientRequest = listener.AcceptTcpClient();
                ProcessClientConnection(clientRequest, cancelationToken);
            }
            listener.Stop();
        }

        private async Task ProcessClientConnection(TcpClient request, CancellationToken cancelationToken)
        {
            if (_configuration.EnableNegotiateStream)
                await AuthenticateClientAsync(request);
            else
            {
                ConnectWithServer(request.GetStream(), request);
            }
        }

        private void ConnectWithServer(Stream stream, TcpClient client)
        {
            var instance = new ServerInstance(_scheduler, _schedulerListener, _eventJobListener, _eventTriggerListener);
            instance.Connect(stream,client);
        }

        public async Task AuthenticateClientAsync(TcpClient clientRequest)
        {
            try
            {
                NetworkStream stream = clientRequest.GetStream();
                
                NegotiateStream authStream = new NegotiateStream(stream, false);
                Task authTask = authStream.AuthenticateAsServerAsync()
                    .ContinueWith(d =>
                    {
                        if (d.IsCompleted && !d.IsFaulted)
                            ConnectWithServer(authStream, clientRequest);
                    },TaskScheduler.Default);
                await authTask;
            }
            catch (AuthenticationException e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Authentication failed - closing connection.");
                clientRequest.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Closing connection.");
                clientRequest.Close();
            }
        }
    }
}