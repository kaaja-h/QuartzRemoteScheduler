using System;
using MessagePack;
using Quartz;
using Quartz.Impl.Triggers;

namespace QuartzRemoteScheduler.Common.Model
{
    [MessagePackObject(keyAsPropertyName:true)]
    class SerializableCronTrigger:SerializableTriggerBase
    {

        public string CronExpressionString { get; set; }
        
        public string TimeZoneId { get; set; }
        
        public SerializableCronTrigger()
        {
            
        }

        public SerializableCronTrigger(ICronTrigger data) : base(data)
        {
            CronExpressionString = data.CronExpressionString;
            TimeZoneId = data.TimeZone.Id;
        }

        protected override TriggerBuilder GetTriggerBuilder()
        {
            var res =  base.GetTriggerBuilder();
            var schedule = CronScheduleBuilder.CronSchedule(CronExpressionString);
            if (!string.IsNullOrEmpty(TimeZoneId))
                schedule.InTimeZone(TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId));
            res.WithSchedule(schedule);
            return res;
        }
    }
}