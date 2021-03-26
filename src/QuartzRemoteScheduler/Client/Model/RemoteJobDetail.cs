using System;
using Quartz;
using QuartzRemoteScheduler.Common.Model;

namespace QuartzRemoteScheduler.Client.Model
{
    internal class RemoteJobDetail :IJobDetail
    {
        public JobBuilder GetJobBuilder()
        {
            return JobBuilder.Create(JobType).RequestRecovery(RequestsRecovery)
                .StoreDurably(Durable)
                .WithDescription(Description)
                .WithIdentity(Key)
                .UsingJobData(JobDataMap);
            
        }

        public IJobDetail Clone()
        {
            return GetJobBuilder().Build();
        }

        public JobKey Key { get;  }
        public string Description { get; }
        public Type JobType { get; }
        public JobDataMap JobDataMap { get;  }
        public bool Durable { get; }
        public bool PersistJobDataAfterExecution { get;  }
        public bool ConcurrentExecutionDisallowed { get;  }
        public bool RequestsRecovery { get; set; }

        public RemoteJobDetail(SerializableJobDetail data)
        {
            Key = data.Key;
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