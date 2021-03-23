using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using QuartzRemoteScheduler.Client;
using QuartzRemoteScheduler.Common.Configuration;
using QuartzRemoteScheduler.Server;

namespace QuartzRemoteScheduler.Test.Common
{
    public class BasicSchedulerFixture:IDisposable
    {

        private static int _maxUsedPort=11000;
        private static object portLock = new object();
        private static int PreparePort()
        {
            
            lock (portLock)
            {
                
                _maxUsedPort++;
                while (!IsPortAvailablePort(_maxUsedPort))
                {
                    _maxUsedPort++;
                }
                return _maxUsedPort;
            }
        }
        
        
        private static bool IsPortAvailablePort(int port)
        {
            IPEndPoint[] endPoints;
            List<int> portArray = new List<int>();

            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();

            //getting active connections
            TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();
            portArray.AddRange(from n in connections
                where n.LocalEndPoint.Port == port
                select n.LocalEndPoint.Port);

            //getting active tcp listners - WCF service listening in tcp
            endPoints = properties.GetActiveTcpListeners();
            portArray.AddRange(from n in endPoints
                where n.Port == port
                select n.Port);

            //getting active udp listeners
            endPoints = properties.GetActiveUdpListeners();
            portArray.AddRange(from n in endPoints
                where n.Port == port
                select n.Port);

            return !portArray.Any();
        }
        
        
        public int Port { get; set; }
        public IScheduler LocalScheduler { get; set; }
        
        public RemoteSchedulerServerConfiguration Conf { get; set; }

        private StdSchedulerFactory PrepareFactory(bool autorize)
        {
            var conf = new NameValueCollection();
            conf["quartz.plugin.server.type"] = typeof(RemoteSchedulerServerPlugin).AssemblyQualifiedName;
            conf["quartz.plugin.server.address"] = IPAddress.Loopback.ToString();
            conf["quartz.plugin.server.port"] = Port.ToString();
            conf["quartz.scheduler.instanceName"] = "MyScheduler" + Port.ToString();
            if (autorize)
                conf["quartz.plugin.server.enableNegotiateStream"] = autorize.ToString();    

            
            Conf = new RemoteSchedulerServerConfiguration(
                IPAddress.Loopback, Port, autorize
            );
            return new StdSchedulerFactory(conf);
        }

        private async Task<IScheduler> PrepareSchedulerAsync(ISchedulerFactory factory)
        {
            var sch = await factory.GetScheduler();
            await sch.Start();

            return sch;
        }

        public BasicSchedulerFixture() 
        {
            Port = PreparePort();
            var factory = PrepareFactory(false);
            var t = PrepareSchedulerAsync(factory);
            t.Wait();
            this.LocalScheduler = t.Result;
        }

        public async Task<IScheduler> GetRemoteSchedulerAsync()
        {
            var sch = new RemoteSchedulerFactory(Conf);
            return await sch.GetScheduler();
        }
        
        
        
        public void Dispose()
        {
            LocalScheduler.Shutdown().Wait();
        }
    }
}