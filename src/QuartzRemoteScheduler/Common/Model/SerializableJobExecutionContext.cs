using System;
using Quartz;

namespace QuartzRemoteScheduler.Common.Model
{
    [MessagePack.MessagePackObject(true)]
    internal class SerializableJobExecutionContext
    {
        public SerializableTrigger Trigger { get; set; }
        public bool Recovering { get; set; }
        
        public SerializableTriggerKey RecoveringTriggerKey { get; set; }
        
        public int RefireCount { get; set; }
        
        public SerializableJobDataMap MergedJobDataMap { get; set; }
        
        public SerializableJobDetail JobDetail { get; set; }
        
        public DateTimeOffset FireTimeUtc { get; set; }
        public DateTimeOffset? ScheduledFireTimeUtc { get; set; }
        public DateTimeOffset? PreviousFireTimeUtc { get; set; }
        public DateTimeOffset? NextFireTimeUtc { get; set; }
        
        public string FireInstanceId { get; set; }
        
        public TimeSpan JobRunTime { get; set; }

        public SerializableJobExecutionContext()
        {
            
        }

        public SerializableJobExecutionContext(IJobExecutionContext data)
        {
            Trigger = new SerializableTrigger(data.Trigger);
            Recovering = data.Recovering;
            RecoveringTriggerKey = data.RecoveringTriggerKey;
            RefireCount = data.RefireCount;
            MergedJobDataMap = new SerializableJobDataMap(data.MergedJobDataMap);
            JobDetail = new SerializableJobDetail(data.JobDetail);
            FireTimeUtc = data.FireTimeUtc;
            ScheduledFireTimeUtc = data.ScheduledFireTimeUtc;
            PreviousFireTimeUtc = data.PreviousFireTimeUtc;
            NextFireTimeUtc = data.NextFireTimeUtc;
            FireInstanceId = data.FireInstanceId;
            JobRunTime = data.JobRunTime;
            
        }
        
    }
}