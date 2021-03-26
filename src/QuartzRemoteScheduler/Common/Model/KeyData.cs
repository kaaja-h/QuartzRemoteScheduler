using MessagePack;
using Quartz.Util;

namespace QuartzRemoteScheduler.Common.Model
{
    [MessagePackObject(keyAsPropertyName: true)]
    abstract class KeyData
    {
        
        public string Name { get; set; }
        public string Group { get; set; }

        

    }
}