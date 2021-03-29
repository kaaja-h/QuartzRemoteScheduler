using System;
using MessagePack;
using Quartz;

namespace QuartzRemoteScheduler.Common.Model
{
    [MessagePackObject(keyAsPropertyName: true)]
    internal class SerializableSchedulerMetaData
    {
        public string SchedulerName { get; set; }

        public string SchedulerInstanceId { get; set; }

        public string SchedulerType { get; set; }

        public bool SchedulerRemote { get; set; }

        public bool Started { get; set; }

        public bool InStandbyMode { get; set; }

        public bool Shutdown { get; set; }

        public string JobStoreType { get; set; }

        public string ThreadPoolType { get; set; }

        public int ThreadPoolSize { get; set; }

        public string Version { get; set; }

        public DateTimeOffset? RunningSince { get; set; }

        public int NumberOfJobsExecuted { get; set; }

        public bool JobStoreSupportsPersistence { get; set; }

        public bool JobStoreClustered { get; set; }

        public SerializableSchedulerMetaData()
        {
        }

        public SerializableSchedulerMetaData(SchedulerMetaData data)
        {
            SchedulerName = data.SchedulerName;
            SchedulerInstanceId = data.SchedulerInstanceId;
            SchedulerType = data.SchedulerType.AssemblyQualifiedName;
            SchedulerRemote = data.SchedulerRemote;
            Started = data.Started;
            InStandbyMode = data.InStandbyMode;
            Shutdown = data.Shutdown;
            JobStoreType = data.JobStoreType.AssemblyQualifiedName;
            ThreadPoolType = data.ThreadPoolType.AssemblyQualifiedName;
            ThreadPoolSize = data.ThreadPoolSize;
            Version = data.Version;
            RunningSince = data.RunningSince;
            NumberOfJobsExecuted = data.NumberOfJobsExecuted;
            JobStoreSupportsPersistence = data.JobStoreSupportsPersistence;
            JobStoreClustered = data.JobStoreClustered;
        }

        public SchedulerMetaData ToData()
        {
            

            return new SchedulerMetaData(
                SchedulerName, SchedulerInstanceId, ConstructType(SchedulerType),
                this.SchedulerRemote, Started, InStandbyMode, Shutdown, RunningSince,
                NumberOfJobsExecuted, ConstructType(JobStoreType), JobStoreSupportsPersistence,
                JobStoreClustered, ConstructType(ThreadPoolType), ThreadPoolSize, Version
            );
        }


        private Type ConstructType(string name)
        {
            try
            {
                return Type.GetType(name);
            }
            catch (Exception)
            {
                // ignored
            }

            return null;
        }
    }
}