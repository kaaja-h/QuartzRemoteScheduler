using MessagePack;
using Quartz;

namespace QuartzRemoteScheduler.Common.Model
{
    [MessagePackObject(keyAsPropertyName: true)]
    class JobDetailData
    {

        public JobDetailData()
        {
            
        }
        
        public JobDetailData(IJobDetail data)
        {
            Key = new SerializableJobKey(data.Key);
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
    }
}