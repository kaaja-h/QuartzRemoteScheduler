using MessagePack;
using Quartz;
using Quartz.Impl.Matchers;

namespace QuartzRemoteScheduler.Common.Model
{
    [MessagePackObject(true)]
    class SerializableJobMatcher:SerializableMatcher<JobKey>
    {

        public SerializableJobMatcher()
        {
            
        }
        
        public SerializableJobMatcher(GroupMatcher<JobKey> data) : base(data)
        {
        }
    }
}