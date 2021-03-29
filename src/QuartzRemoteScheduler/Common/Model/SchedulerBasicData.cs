using MessagePack;
using Quartz;

namespace QuartzRemoteScheduler.Common.Model
{
    [MessagePackObject(keyAsPropertyName: true)]
    internal class SchedulerBasicData
    {
      
        
        public bool IsStarted { get; set; }
        public string SchedulerName { get; set; }
        public string SchedulerInstanceId { get; set; }
        
        public bool InStandbyMode { get; set; }
        public bool IsShutdown { get; set; }
        
        public SchedulerBasicData()
        {
            
        }

        public SchedulerBasicData(IScheduler scheduler)
        {
            IsShutdown = scheduler.IsShutdown;
            IsStarted = scheduler.IsStarted;
            SchedulerName = scheduler.SchedulerName;
            SchedulerInstanceId = scheduler.SchedulerInstanceId;
            InStandbyMode = scheduler.InStandbyMode;
        }
    }
}