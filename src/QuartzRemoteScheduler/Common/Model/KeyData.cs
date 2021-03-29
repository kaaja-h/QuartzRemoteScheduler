using MessagePack;

namespace QuartzRemoteScheduler.Common.Model
{
    [MessagePackObject(keyAsPropertyName: true)]
    internal abstract class KeyData
    {
        
        public string Name { get; set; }
        public string Group { get; set; }

        

    }
}