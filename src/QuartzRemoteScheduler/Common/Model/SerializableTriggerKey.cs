using Quartz;

namespace QuartzRemoteScheduler.Common.Model
{
    internal class SerializableTriggerKey:KeyData
    {
        

        public static implicit operator TriggerKey(SerializableTriggerKey d) => new TriggerKey(d.Name, d.Group);

        public static implicit operator SerializableTriggerKey(TriggerKey d) => new SerializableTriggerKey()
        {
            Group = d.Group,
            Name = d.Name
        };
    }
    
    
}