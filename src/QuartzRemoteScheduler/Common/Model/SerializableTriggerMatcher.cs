using MessagePack;
using Quartz;
using Quartz.Impl.Matchers;

namespace QuartzRemoteScheduler.Common.Model
{
    [MessagePackObject(true)]
    internal class SerializableTriggerMatcher:SerializableMatcher<TriggerKey>
    {
        public SerializableTriggerMatcher()
        {
            
        }
        
        public SerializableTriggerMatcher(GroupMatcher<TriggerKey> data) : base(data)
        {
        }
    }
}