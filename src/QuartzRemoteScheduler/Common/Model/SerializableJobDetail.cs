using System;
using MessagePack;
using Quartz;

namespace QuartzRemoteScheduler.Common.Model
{
    [MessagePackObject(keyAsPropertyName: true)]
    class SerializableJobDetail
    {

        public SerializableJobDetail()
        {
            
        }
        
        public SerializableJobDetail(IJobDetail data)
        {
            Key = data.Key;
            Description = data.Description;
            JobType = data.JobType.AssemblyQualifiedName;
            SerializableJobDataMap = (data.JobDataMap==null)?null:new SerializableJobDataMap(data.JobDataMap);
            Durable = data.Durable;
            PersistJobDataAfterExecution = data.PersistJobDataAfterExecution;
            ConcurrentExecutionDisallowed = data.ConcurrentExecutionDisallowed;
            RequestsRecovery = data.RequestsRecovery;
        }
        
        public SerializableJobKey Key { get; set; }
        public string Description { get; set; }
        public string JobType { get; set; }
        public SerializableJobDataMap SerializableJobDataMap { get; set; }
        public bool Durable { get; set; }
        public bool PersistJobDataAfterExecution { get; set; }
        public bool ConcurrentExecutionDisallowed { get; set; }
        public bool RequestsRecovery { get; set; }

        public IJobDetail GetJobDetail()
        {
            var type = Type.GetType(JobType);
            var builder = JobBuilder.Create(type);
            builder.RequestRecovery(RequestsRecovery)
                .StoreDurably(Durable)
                .WithDescription(Description)
                .WithIdentity(Key)
                .SetJobData(SerializableJobDataMap.GetMap());
            return builder.Build();

        }
    }
}