using Quartz;

namespace QuartzRemoteScheduler.Common.Model
{
    [MessagePack.MessagePackObject(true)]
    internal class SerializableTrigger
    {
        
        
        
        public SerializableTriggerBase SerializableTriggerBase { get; set; }
        public SerializableCronTrigger SerializableCronTrigger { get; set; }
        public SerializableSimpleTrigger SerializableSimpleTrigger { get; set; }

        public ITrigger Trigger
        {
            get
            {
                if (SerializableCronTrigger != null)
                    return SerializableCronTrigger.ToTrigger();
                if (SerializableSimpleTrigger != null)
                    return SerializableSimpleTrigger.ToTrigger();
                return null;
            }
        }
        
        public SerializableTrigger(ITrigger data)
        {
            if (data is ICronTrigger ct)
                SerializableCronTrigger =  new SerializableCronTrigger(ct);
            else if (data is ISimpleTrigger st)
                SerializableSimpleTrigger = new SerializableSimpleTrigger(st);
            else
                SerializableTriggerBase = new SerializableTriggerBase(data);
        }

        public SerializableTrigger()
        {
            
        }
        
        

 





        
        
    }
}