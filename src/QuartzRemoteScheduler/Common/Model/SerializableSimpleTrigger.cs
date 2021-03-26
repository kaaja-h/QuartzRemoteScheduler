using System;
using MessagePack;
using Quartz;

namespace QuartzRemoteScheduler.Common.Model
{
    [MessagePackObject(keyAsPropertyName:true)]
    class SerializableSimpleTrigger : SerializableTriggerBase
    {
        
        public int RepeatCount { get; set; }

        public TimeSpan RepeatInterval { get; set; }
       
        public int TimesTriggered { get; set; }

        public SerializableSimpleTrigger()
        {
            
        }

        public SerializableSimpleTrigger(ISimpleTrigger data) : base(data)
        {
            RepeatCount = data.RepeatCount;
            RepeatInterval = data.RepeatInterval;
            TimesTriggered = data.TimesTriggered;
        }

        protected override TriggerBuilder GetTriggerBuilder()
        {
            var res = base.GetTriggerBuilder();
            var schedule = SimpleScheduleBuilder.Create();
            schedule.WithRepeatCount(RepeatCount);
            schedule.WithInterval(RepeatInterval);
            res.WithSchedule(schedule);
            return res;
        }
    }
}