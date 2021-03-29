using MessagePack;

namespace QuartzRemoteScheduler.Common.Model
{
    [MessagePackObject(keyAsPropertyName: true)]
    internal class SerializableJobDataMapItem
    {
        public string Key { get; set; }
        public string StringValue { get; set; }
        public int? IntValue { get; set; }
        public long? LongValue { get; set; }
        public bool? BoolValue { get; set; }
        
    }
}