using System;
using MessagePack;
using Quartz;

namespace QuartzRemoteScheduler.Common.Model
{

    [MessagePackObject(keyAsPropertyName:true)]
    internal class SerializableTriggerBase
    {
        
        public SerializableTriggerKey Key { get; set; }
        
        public SerializableJobKey JobKey { get; set; }
        
        public string Description { get; set; }
        
        public string CalendarName { get; set; }
        
        public SerializableJobDataMap JobDataMap { get; set; }
        
        public DateTimeOffset? FinalFireTimeUtc { get; set; }
        
        public int MisfireInstruction { get; set; }
        
        public DateTimeOffset? EndTimeUtc { get; set; }
        
        public DateTimeOffset StartTimeUtc { get; set; }
        
        public int Priority { get; set; }
        
        public bool HasMillisecondPrecision { get; set; }

        public SerializableTriggerBase()
        {
        }

        public SerializableTriggerBase(ITrigger data)
        {
            Key = data.Key;
            JobKey = data.JobKey;
            Description = data.Description;
            CalendarName = data.CalendarName;
            JobDataMap = new SerializableJobDataMap(data.JobDataMap);
            FinalFireTimeUtc = data.FinalFireTimeUtc;
            MisfireInstruction = data.MisfireInstruction;
            EndTimeUtc = data.EndTimeUtc;
            StartTimeUtc = data.StartTimeUtc;
            Priority = data.Priority;
            HasMillisecondPrecision = data.HasMillisecondPrecision;
        }

        public  ITrigger ToTrigger()
        {
            return GetTriggerBuilder().Build();
        }

        protected virtual TriggerBuilder GetTriggerBuilder()
        {
            var builder = TriggerBuilder.Create();
            if (!string.IsNullOrEmpty(Description))
                builder.WithDescription(Description);
            builder.WithIdentity(Key);
            builder.ForJob(JobKey);
            builder.WithPriority(Priority);
            if (!string.IsNullOrEmpty(CalendarName))
                builder.ModifiedByCalendar(CalendarName);
            builder.UsingJobData(JobDataMap.GetMap());
            if (EndTimeUtc.HasValue)
                builder.EndAt(EndTimeUtc);
            builder.StartAt(StartTimeUtc);
            return builder;
        }
    }
}