<a name='assembly'></a>
# QuartzRemoteScheduler

## Contents

- [RemoteSchedulerFactory](#T-QuartzRemoteScheduler-Client-RemoteSchedulerFactory 'QuartzRemoteScheduler.Client.RemoteSchedulerFactory')
  - [#ctor(conf)](#M-QuartzRemoteScheduler-Client-RemoteSchedulerFactory-#ctor-QuartzRemoteScheduler-Common-Configuration-RemoteSchedulerServerConfiguration- 'QuartzRemoteScheduler.Client.RemoteSchedulerFactory.#ctor(QuartzRemoteScheduler.Common.Configuration.RemoteSchedulerServerConfiguration)')
  - [GetAllSchedulers(cancellationToken)](#M-QuartzRemoteScheduler-Client-RemoteSchedulerFactory-GetAllSchedulers-System-Threading-CancellationToken- 'QuartzRemoteScheduler.Client.RemoteSchedulerFactory.GetAllSchedulers(System.Threading.CancellationToken)')
  - [GetScheduler(cancellationToken)](#M-QuartzRemoteScheduler-Client-RemoteSchedulerFactory-GetScheduler-System-Threading-CancellationToken- 'QuartzRemoteScheduler.Client.RemoteSchedulerFactory.GetScheduler(System.Threading.CancellationToken)')
  - [GetScheduler(schedName,cancellationToken)](#M-QuartzRemoteScheduler-Client-RemoteSchedulerFactory-GetScheduler-System-String,System-Threading-CancellationToken- 'QuartzRemoteScheduler.Client.RemoteSchedulerFactory.GetScheduler(System.String,System.Threading.CancellationToken)')
- [RemoteSchedulerServerConfiguration](#T-QuartzRemoteScheduler-Common-Configuration-RemoteSchedulerServerConfiguration 'QuartzRemoteScheduler.Common.Configuration.RemoteSchedulerServerConfiguration')
  - [#ctor(address,port,enableNegotiateStream)](#M-QuartzRemoteScheduler-Common-Configuration-RemoteSchedulerServerConfiguration-#ctor-System-String,System-Int32,System-Boolean- 'QuartzRemoteScheduler.Common.Configuration.RemoteSchedulerServerConfiguration.#ctor(System.String,System.Int32,System.Boolean)')
  - [#ctor(address,port,enableNegotiateStream)](#M-QuartzRemoteScheduler-Common-Configuration-RemoteSchedulerServerConfiguration-#ctor-System-Net-IPAddress,System-Int32,System-Boolean- 'QuartzRemoteScheduler.Common.Configuration.RemoteSchedulerServerConfiguration.#ctor(System.Net.IPAddress,System.Int32,System.Boolean)')
  - [Address](#P-QuartzRemoteScheduler-Common-Configuration-RemoteSchedulerServerConfiguration-Address 'QuartzRemoteScheduler.Common.Configuration.RemoteSchedulerServerConfiguration.Address')
  - [EnableNegotiateStream](#P-QuartzRemoteScheduler-Common-Configuration-RemoteSchedulerServerConfiguration-EnableNegotiateStream 'QuartzRemoteScheduler.Common.Configuration.RemoteSchedulerServerConfiguration.EnableNegotiateStream')
  - [Port](#P-QuartzRemoteScheduler-Common-Configuration-RemoteSchedulerServerConfiguration-Port 'QuartzRemoteScheduler.Common.Configuration.RemoteSchedulerServerConfiguration.Port')
- [RemoteSchedulerServerPlugin](#T-QuartzRemoteScheduler-Server-RemoteSchedulerServerPlugin 'QuartzRemoteScheduler.Server.RemoteSchedulerServerPlugin')
  - [Address](#P-QuartzRemoteScheduler-Server-RemoteSchedulerServerPlugin-Address 'QuartzRemoteScheduler.Server.RemoteSchedulerServerPlugin.Address')
  - [EnableNegotiateStream](#P-QuartzRemoteScheduler-Server-RemoteSchedulerServerPlugin-EnableNegotiateStream 'QuartzRemoteScheduler.Server.RemoteSchedulerServerPlugin.EnableNegotiateStream')
  - [Port](#P-QuartzRemoteScheduler-Server-RemoteSchedulerServerPlugin-Port 'QuartzRemoteScheduler.Server.RemoteSchedulerServerPlugin.Port')
  - [Initialize(pluginName,scheduler,cancellationToken)](#M-QuartzRemoteScheduler-Server-RemoteSchedulerServerPlugin-Initialize-System-String,Quartz-IScheduler,System-Threading-CancellationToken- 'QuartzRemoteScheduler.Server.RemoteSchedulerServerPlugin.Initialize(System.String,Quartz.IScheduler,System.Threading.CancellationToken)')
  - [Shutdown()](#M-QuartzRemoteScheduler-Server-RemoteSchedulerServerPlugin-Shutdown-System-Threading-CancellationToken- 'QuartzRemoteScheduler.Server.RemoteSchedulerServerPlugin.Shutdown(System.Threading.CancellationToken)')
  - [Start()](#M-QuartzRemoteScheduler-Server-RemoteSchedulerServerPlugin-Start-System-Threading-CancellationToken- 'QuartzRemoteScheduler.Server.RemoteSchedulerServerPlugin.Start(System.Threading.CancellationToken)')

<a name='T-QuartzRemoteScheduler-Client-RemoteSchedulerFactory'></a>
## RemoteSchedulerFactory `type`

##### Namespace

QuartzRemoteScheduler.Client

##### Summary

Class for creating scheduler client for remote server

<a name='M-QuartzRemoteScheduler-Client-RemoteSchedulerFactory-#ctor-QuartzRemoteScheduler-Common-Configuration-RemoteSchedulerServerConfiguration-'></a>
### #ctor(conf) `constructor`

##### Summary

Constructor with configuration

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| conf | [QuartzRemoteScheduler.Common.Configuration.RemoteSchedulerServerConfiguration](#T-QuartzRemoteScheduler-Common-Configuration-RemoteSchedulerServerConfiguration 'QuartzRemoteScheduler.Common.Configuration.RemoteSchedulerServerConfiguration') | Connection configuration |

<a name='M-QuartzRemoteScheduler-Client-RemoteSchedulerFactory-GetAllSchedulers-System-Threading-CancellationToken-'></a>
### GetAllSchedulers(cancellationToken) `method`

##### Summary

[GetAllSchedulers](#M-Quartz-ISchedulerFactory-GetAllSchedulers-System-Threading-CancellationToken- 'Quartz.ISchedulerFactory.GetAllSchedulers(System.Threading.CancellationToken)')

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| cancellationToken | [System.Threading.CancellationToken](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Threading.CancellationToken 'System.Threading.CancellationToken') |  |

<a name='M-QuartzRemoteScheduler-Client-RemoteSchedulerFactory-GetScheduler-System-Threading-CancellationToken-'></a>
### GetScheduler(cancellationToken) `method`

##### Summary

[GetScheduler](#M-Quartz-ISchedulerFactory-GetScheduler-System-Threading-CancellationToken- 'Quartz.ISchedulerFactory.GetScheduler(System.Threading.CancellationToken)')

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| cancellationToken | [System.Threading.CancellationToken](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Threading.CancellationToken 'System.Threading.CancellationToken') |  |

<a name='M-QuartzRemoteScheduler-Client-RemoteSchedulerFactory-GetScheduler-System-String,System-Threading-CancellationToken-'></a>
### GetScheduler(schedName,cancellationToken) `method`

##### Summary

[GetScheduler](#M-Quartz-ISchedulerFactory-GetScheduler-System-String,System-Threading-CancellationToken- 'Quartz.ISchedulerFactory.GetScheduler(System.String,System.Threading.CancellationToken)')

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| schedName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') |  |
| cancellationToken | [System.Threading.CancellationToken](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Threading.CancellationToken 'System.Threading.CancellationToken') |  |

<a name='T-QuartzRemoteScheduler-Common-Configuration-RemoteSchedulerServerConfiguration'></a>
## RemoteSchedulerServerConfiguration `type`

##### Namespace

QuartzRemoteScheduler.Common.Configuration

##### Summary

Configuration for connection to server

<a name='M-QuartzRemoteScheduler-Common-Configuration-RemoteSchedulerServerConfiguration-#ctor-System-String,System-Int32,System-Boolean-'></a>
### #ctor(address,port,enableNegotiateStream) `constructor`

##### Summary



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| address | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | server ip address |
| port | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | server port |
| enableNegotiateStream | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | use negotiate stream [NegotiateStream](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Net.Security.NegotiateStream 'System.Net.Security.NegotiateStream') |

<a name='M-QuartzRemoteScheduler-Common-Configuration-RemoteSchedulerServerConfiguration-#ctor-System-Net-IPAddress,System-Int32,System-Boolean-'></a>
### #ctor(address,port,enableNegotiateStream) `constructor`

##### Summary



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| address | [System.Net.IPAddress](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Net.IPAddress 'System.Net.IPAddress') | server ip address |
| port | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | server port |
| enableNegotiateStream | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | use negotiate stream [NegotiateStream](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Net.Security.NegotiateStream 'System.Net.Security.NegotiateStream') |

<a name='P-QuartzRemoteScheduler-Common-Configuration-RemoteSchedulerServerConfiguration-Address'></a>
### Address `property`

##### Summary

Server Ip address

<a name='P-QuartzRemoteScheduler-Common-Configuration-RemoteSchedulerServerConfiguration-EnableNegotiateStream'></a>
### EnableNegotiateStream `property`

##### Summary

Use Negotiate stream
[NegotiateStream](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Net.Security.NegotiateStream 'System.Net.Security.NegotiateStream')

<a name='P-QuartzRemoteScheduler-Common-Configuration-RemoteSchedulerServerConfiguration-Port'></a>
### Port `property`

##### Summary

Server port

<a name='T-QuartzRemoteScheduler-Server-RemoteSchedulerServerPlugin'></a>
## RemoteSchedulerServerPlugin `type`

##### Namespace

QuartzRemoteScheduler.Server

##### Summary

Plugin

<a name='P-QuartzRemoteScheduler-Server-RemoteSchedulerServerPlugin-Address'></a>
### Address `property`

##### Summary

server address

<a name='P-QuartzRemoteScheduler-Server-RemoteSchedulerServerPlugin-EnableNegotiateStream'></a>
### EnableNegotiateStream `property`

##### Summary

use negotiate stream [NegotiateStream](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Net.Security.NegotiateStream 'System.Net.Security.NegotiateStream')

<a name='P-QuartzRemoteScheduler-Server-RemoteSchedulerServerPlugin-Port'></a>
### Port `property`

##### Summary

server port

<a name='M-QuartzRemoteScheduler-Server-RemoteSchedulerServerPlugin-Initialize-System-String,Quartz-IScheduler,System-Threading-CancellationToken-'></a>
### Initialize(pluginName,scheduler,cancellationToken) `method`

##### Summary

[Initialize](#M-Quartz-Spi-ISchedulerPlugin-Initialize-System-String,Quartz-IScheduler,System-Threading-CancellationToken- 'Quartz.Spi.ISchedulerPlugin.Initialize(System.String,Quartz.IScheduler,System.Threading.CancellationToken)')

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| pluginName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') |  |
| scheduler | [Quartz.IScheduler](#T-Quartz-IScheduler 'Quartz.IScheduler') |  |
| cancellationToken | [System.Threading.CancellationToken](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Threading.CancellationToken 'System.Threading.CancellationToken') |  |

<a name='M-QuartzRemoteScheduler-Server-RemoteSchedulerServerPlugin-Shutdown-System-Threading-CancellationToken-'></a>
### Shutdown() `method`

##### Parameters

This method has no parameters.

<a name='M-QuartzRemoteScheduler-Server-RemoteSchedulerServerPlugin-Start-System-Threading-CancellationToken-'></a>
### Start() `method`

##### Parameters

This method has no parameters.
