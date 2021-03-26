namespace QuartzRemoteScheduler.Common.Model
{
    [MessagePack.MessagePackObject(true)]
    class JobTriggersPairsItems
    {
        public SerializableJobDetail Detail { get; set; }
        public SerializableTrigger[] Triggers { get; set; }
        
    }
}