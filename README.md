# QuartzRemoteScheduler

Plugin for [quartz.net](https://quartz-scheduler.net) scheduler for enablign remote scheduler control

## Technology
- netstandard2.0 - works in net462, net472, net5.0, ...
- TCP listening - supports [NegotiateStream](https://docs.microsoft.com/en-us/dotnet/api/system.net.security.negotiatestream?view=net-5.0)
- Transports data using [StreamJsonRpc](https://github.com/microsoft/vs-streamjsonrpc) with [MessagePack-CSharp](https://github.com/neuecc/MessagePack-CSharp) formating


## Server
enable plugin using configuration 

``` c#
var conf = new NameValueCollection();
conf["quartz.plugin.remoteScheduler.type"] = typeof(RemoteSchedulerServerPlugin).AssemblyQualifiedName;
conf["quartz.plugin.remoteScheduler.address"] = "0.0.0.0";
conf["quartz.plugin.remoteScheduler.port"] = 12345;
conf["quartz.plugin.remoteScheduler.enableNegotiateStream"] = "false";
var factory = new StdSchedulerFactory(conf);
var scheduler = await factory.GetScheduler();  
```

## Client
simple create scheduler instance using 
``` c#
var conf = new RemoteSchedulerServerConfiguration("127.0.0.1", 12345, false);
var schedulerFactory = new RemoteSchedulerFactory(conf);
var scheduler = await sch.GetScheduler();
```

## What is done
- All ISchedule implementations excluding one which works with calendars
- works remote job adding, scheduling, pausing, ...

## What is not done
- Listeners
- ICalendar stuff

## Future improvements
- Add job which type is not present on client
- Security


## Api
Api can be found [here](api/QuartzRemoteScheduler.md)
