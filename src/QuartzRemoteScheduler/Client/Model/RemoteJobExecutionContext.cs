using System;
using System.Collections.Generic;
using System.Threading;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using QuartzRemoteScheduler.Common.Model;

namespace QuartzRemoteScheduler.Client.Model
{
    internal class RemoteJobExecutionContext:IJobExecutionContext, ICancellableJobExecutionContext
    {

        public void Put(object key, object objectValue)
        {
            throw new NotImplementedException();
        }

        public object Get(object key)
        {
            throw new NotImplementedException();
        }

        public IScheduler Scheduler { get; }
        public ITrigger Trigger { get; }
        public ICalendar Calendar { get; }
        public bool Recovering { get; }
        public TriggerKey RecoveringTriggerKey { get; }
        public int RefireCount { get; }
        public JobDataMap MergedJobDataMap { get; }
        public IJobDetail JobDetail { get; }
        public IJob JobInstance { get; }
        public DateTimeOffset FireTimeUtc { get; }
        public DateTimeOffset? ScheduledFireTimeUtc { get; }
        public DateTimeOffset? PreviousFireTimeUtc { get; }
        public DateTimeOffset? NextFireTimeUtc { get; }
        public string FireInstanceId { get; }
        public object? Result { get; set; }
        public TimeSpan JobRunTime { get; }
        public CancellationToken CancellationToken { get; }
        public void Cancel()
        {
            throw new NotImplementedException();
        }
        
        

        public RemoteJobExecutionContext(SerializableJobExecutionContext data, IScheduler scheduler)
        {
            Scheduler = scheduler;
            Trigger = data.Trigger.Trigger;
            Recovering = data.Recovering;
            RecoveringTriggerKey = data.RecoveringTriggerKey;
            RefireCount = data.RefireCount;
            MergedJobDataMap = data.MergedJobDataMap.GetMap();
            JobDetail = data.JobDetail.GetJobDetail();
            FireTimeUtc = data.FireTimeUtc;
            ScheduledFireTimeUtc = data.ScheduledFireTimeUtc;
            PreviousFireTimeUtc = data.PreviousFireTimeUtc;
            NextFireTimeUtc = data.NextFireTimeUtc;
            FireInstanceId = data.FireInstanceId;
            JobRunTime = data.JobRunTime;

        }
    }
}