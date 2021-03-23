using System.Net;
using System.Net.Security;

namespace QuartzRemoteScheduler.Common.Configuration
{
    /// <summary>
    /// Configuration for connection to server
    /// </summary>
    public class RemoteSchedulerServerConfiguration
    {
        /// <summary>
        /// Server Ip address
        /// </summary>
        public IPAddress Address { get; }

        /// <summary>
        /// Server port
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// Use Negotiate stream
        /// <see cref="NegotiateStream"/>
        /// </summary>
        public bool EnableNegotiateStream { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address">server ip address</param>
        /// <param name="port">server port</param>
        /// <param name="enableNegotiateStream">use negotiate stream <see cref="NegotiateStream"/></param>
        public RemoteSchedulerServerConfiguration(string address, int port, bool enableNegotiateStream) : this(
            IPAddress.Parse(address), port, enableNegotiateStream)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address">server ip address</param>
        /// <param name="port">server port</param>
        /// <param name="enableNegotiateStream">use negotiate stream <see cref="NegotiateStream"/></param>
        public RemoteSchedulerServerConfiguration(IPAddress address, int port, bool enableNegotiateStream)
        {
            Address = address;
            Port = port;
            EnableNegotiateStream = enableNegotiateStream;
        }
    }
}