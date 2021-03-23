using System;
using MessagePack;
using Quartz;

namespace QuartzRemoteScheduler.Common.Model
{
    [MessagePackObject(keyAsPropertyName:true)]
    class SerializableTrigger
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

        public SerializableTrigger()
        {
        }

        SerializableTrigger(ITrigger data)
        {
            Key = new SerializableTriggerKey(data.Key);
            JobKey = new SerializableJobKey(data.JobKey);
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
    }
}