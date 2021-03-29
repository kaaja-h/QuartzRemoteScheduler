namespace QuartzRemoteScheduler.Common.Model
{
    [MessagePack.MessagePackObject(true)]
    internal class JobTriggersPairsItems
    {
        public SerializableJobDetail Detail { get; set; }
        public SerializableTrigger[] Triggers { get; set; }
        
    }
}