using MessagePack;
using Quartz;

namespace QuartzRemoteScheduler.Common.Model
{
    [MessagePackObject(keyAsPropertyName: true)]
    class SerializableJobKey:KeyData
    {
        public static implicit operator JobKey(SerializableJobKey d) => new JobKey(d.Name, d.Group);

        public static implicit operator SerializableJobKey(JobKey d) => new SerializableJobKey()
        {
            Group = d.Group,
            Name = d.Name
        };
    }
}