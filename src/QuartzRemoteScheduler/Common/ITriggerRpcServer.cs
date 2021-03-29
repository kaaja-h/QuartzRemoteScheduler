using System;
using System.Threading.Tasks;
using QuartzRemoteScheduler.Common.Model;

namespace QuartzRemoteScheduler.Common
{
    internal interface ITriggerRpcServer
    {
        Task<bool> GetMayFireAgainAsync(SerializableTriggerKey key);

        Task<DateTimeOffset?> GetNextFireTimeUtcAsync(SerializableTriggerKey key);
        
        Task<DateTimeOffset?> GetPreviousFireTimeUtcAsync(SerializableTriggerKey key);

        Task<DateTimeOffset?> GetFireTimeAfterAsync(SerializableTriggerKey key, DateTimeOffset? afterTime);



        Task<string> GetCronExpressionSummaryAsync(SerializableTriggerKey key);


    }
}