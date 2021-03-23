using MessagePack;
using Quartz;

namespace QuartzRemoteScheduler.Common.Model
{
    [MessagePackObject(keyAsPropertyName: true)]
    class SerializableJobKey:KeyData<JobKey>
    {
        public SerializableJobKey()
        {
            
        }

        public SerializableJobKey(JobKey t):base(t)
        {
            
        }
        
        public override JobKey ToKey()
        {
            return new JobKey(Name, Group);
        }
    }
}