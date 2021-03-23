using System;
using System.Threading.Tasks;
using Quartz;
using QuartzRemoteScheduler.Common;
using QuartzRemoteScheduler.Common.Model;

namespace QuartzRemoteScheduler.Server
{
    
    
    
    class TriggerRpcServer:ITriggerRpcServer
    {
        private readonly IScheduler _scheduler;

        public TriggerRpcServer(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }
        
        
        
        

        public async Task<bool> GetMayFireAgainAsync(SerializableTriggerKey key)
        {
            return (await _scheduler.GetTrigger(key.ToKey())).GetMayFireAgain();
            
        }

        public async Task<DateTimeOffset?> GetNextFireTimeUtcAsync(SerializableTriggerKey key)
        {
            return (await _scheduler.GetTrigger(key.ToKey())).GetNextFireTimeUtc();
        }

        public async Task<DateTimeOffset?> GetPreviousFireTimeUtcAsync(SerializableTriggerKey key)
        {
            return (await _scheduler.GetTrigger(key.ToKey())).GetPreviousFireTimeUtc();
        }

        public async Task<DateTimeOffset?> GetFireTimeAfterAsync(SerializableTriggerKey key, DateTimeOffset? afterTime)
        {
            return (await _scheduler.GetTrigger(key.ToKey())).GetFireTimeAfter(afterTime);
        }
    }
}