using System;
using Quartz;
using QuartzRemoteScheduler.Common.Model;

namespace QuartzRemoteScheduler.Client.Model.Trigger
{
    internal class RemoteConnectedCronTrigger : RemoteConnectedTrigger, ICronTrigger
    {
        public string GetExpressionSummary()
        {
            return _cronExpression.GetExpressionSummary();
        }

        private string _cronExpressionString;
        private CronExpression _cronExpression;

        public string CronExpressionString
        {
            get
            {
                return _cronExpressionString;
            }
            set
            {
                _cronExpression = new CronExpression(value);
                _cronExpression.TimeZone = TimeZone;
                _cronExpressionString = value;
            }
        }

        public TimeZoneInfo TimeZone
        {
            get;
            set;
        }

        public RemoteConnectedCronTrigger(SerializableCronTrigger data, Connector connector) : base(data, connector)
        {
            TimeZone = TimeZoneInfo.FindSystemTimeZoneById(data.TimeZoneId);
            CronExpressionString = data.CronExpressionString;
        }

        public override IScheduleBuilder GetScheduleBuilder()
        {
            var res = CronScheduleBuilder.CronSchedule(CronExpressionString);
            res.InTimeZone(TimeZone);
            return res;
        }
    }
}