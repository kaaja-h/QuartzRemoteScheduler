using System;
using Quartz;
using QuartzRemoteScheduler.Common.Model;

namespace QuartzRemoteScheduler.Client.Model
{
    internal class RemoteJobDetail :IJobDetail
    {
        public JobBuilder GetJobBuilder()
        {
            throw new NotImplementedException();
        }

        public IJobDetail Clone()
        {
            throw new NotImplementedException();
        }

        public JobKey Key { get; set; }
        public string Description { get; set; }
        public Type JobType { get; set; }
        public JobDataMap JobDataMap { get; set; }
        public bool Durable { get; set; }
        public bool PersistJobDataAfterExecution { get; set; }
        public bool ConcurrentExecutionDisallowed { get; set; }
        public bool RequestsRecovery { get; set; }

        public RemoteJobDetail(JobDetailData data)
        {
            Key = data.Key.ToKey();
            Description = data.Description;
            JobType = Type.GetType(data.JobType);
            Durable = data.Durable;
            PersistJobDataAfterExecution = data.PersistJobDataAfterExecution;
            ConcurrentExecutionDisallowed = data.ConcurrentExecutionDisallowed;
            RequestsRecovery = data.RequestsRecovery;
            JobDataMap = data.SerializableJobDataMap.GetMap();
        }
    }
}