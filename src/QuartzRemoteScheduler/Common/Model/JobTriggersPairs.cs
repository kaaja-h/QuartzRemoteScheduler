using System.Collections.Generic;

namespace QuartzRemoteScheduler.Common.Model
{
    [MessagePack.MessagePackObject(true)]
    class JobTriggersPairs
    {
        public JobTriggersPairsItems[] Data { get; set; }
    }
}