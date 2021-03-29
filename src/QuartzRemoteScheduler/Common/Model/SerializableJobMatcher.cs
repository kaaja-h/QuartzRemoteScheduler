using MessagePack;
using Quartz;
using Quartz.Impl.Matchers;

namespace QuartzRemoteScheduler.Common.Model
{
    [MessagePackObject(true)]
    internal class SerializableJobMatcher:SerializableMatcher<JobKey>
    {

        public SerializableJobMatcher()
        {
            
        }
        
        public SerializableJobMatcher(GroupMatcher<JobKey> data) : base(data)
        {
        }
    }
}