namespace QuartzRemoteScheduler.Common.Model
{
    [MessagePack.MessagePackObject(true)]
    internal class JobTriggersPairs
    {
        public JobTriggersPairsItems[] Data { get; set; }
    }
}