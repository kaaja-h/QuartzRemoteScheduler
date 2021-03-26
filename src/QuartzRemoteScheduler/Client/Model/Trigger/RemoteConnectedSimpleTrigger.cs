using System;
using Quartz;
using QuartzRemoteScheduler.Common.Model;

namespace QuartzRemoteScheduler.Client.Model.Trigger
{
    class RemoteConnectedSimpleTrigger : RemoteConnectedTrigger, ISimpleTrigger
    {
        public RemoteConnectedSimpleTrigger(SerializableSimpleTrigger data, Connector connector) : base(data, connector)
        {
            RepeatCount = data.RepeatCount;
            RepeatInterval = data.RepeatInterval;
            TimesTriggered = data.TimesTriggered;
        }

        public int RepeatCount { get; set; }
        public TimeSpan RepeatInterval { get; set; }
        public int TimesTriggered { get; set; }

        public override IScheduleBuilder GetScheduleBuilder()
        {
            var res = SimpleScheduleBuilder.Create();
            res.WithInterval(RepeatInterval).WithRepeatCount(RepeatCount);
            return res;
        }
    }
}