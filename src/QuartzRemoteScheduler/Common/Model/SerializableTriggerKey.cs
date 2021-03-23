using Quartz;

namespace QuartzRemoteScheduler.Common.Model
{
    class SerializableTriggerKey:KeyData<TriggerKey>
    {
        public SerializableTriggerKey()
        {
            
        }

        public SerializableTriggerKey(TriggerKey t):base(t)
        {
            
        }
        
        public override TriggerKey ToKey()
        {
            return new TriggerKey(Name, Group);
        }
    }
}