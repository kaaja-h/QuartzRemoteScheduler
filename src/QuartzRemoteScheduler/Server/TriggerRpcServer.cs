using System;
using System.Threading.Tasks;
using Quartz;
using QuartzRemoteScheduler.Common;
using QuartzRemoteScheduler.Common.Model;

namespace QuartzRemoteScheduler.Server
{
    internal class TriggerRpcServer:ITriggerRpcServer
    {
        private readonly IScheduler _scheduler;

        public TriggerRpcServer(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }
        
        
        
        

        public async Task<bool> GetMayFireAgainAsync(SerializableTriggerKey key)
        {
            return ((await _scheduler.GetTrigger(key))?.GetMayFireAgain()).GetValueOrDefault();
            
        }

        public async Task<DateTimeOffset?> GetNextFireTimeUtcAsync(SerializableTriggerKey key)
        {
            return (await _scheduler.GetTrigger(key))?.GetNextFireTimeUtc();
        }

        public async Task<DateTimeOffset?> GetPreviousFireTimeUtcAsync(SerializableTriggerKey key)
        {
            return (await _scheduler.GetTrigger(key))?.GetPreviousFireTimeUtc();
        }

        public async Task<DateTimeOffset?> GetFireTimeAfterAsync(SerializableTriggerKey key, DateTimeOffset? afterTime)
        {
            return (await _scheduler.GetTrigger(key))?.GetFireTimeAfter(afterTime);
        }
        

        public async Task<string> GetCronExpressionSummaryAsync(SerializableTriggerKey key)
        {
            var tr = (await _scheduler.GetTrigger(key))as ICronTrigger;
            return tr?.GetExpressionSummary();
        }


    }
}