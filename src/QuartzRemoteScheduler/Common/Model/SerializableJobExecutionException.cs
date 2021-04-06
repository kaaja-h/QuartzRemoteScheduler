using Quartz;

namespace QuartzRemoteScheduler.Common.Model
{
    [MessagePack.MessagePackObject(true)]
    class SerializableJobExecutionException
    {
        public bool UnscheduleFiringTrigger { set; get; }
        public bool UnscheduleAllTriggers { set; get; }
        public string Message { get; set; }
        public bool RefireImmediately { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return Value;
        }


        public SerializableJobExecutionException()
        {
            
        }
        
        public SerializableJobExecutionException(JobExecutionException e)
        {
            UnscheduleAllTriggers = e.UnscheduleAllTriggers;
            UnscheduleFiringTrigger = e.UnscheduleFiringTrigger;
            RefireImmediately = e.RefireImmediately;
            Message = e.Message;
            Value = e.ToString();
        }

        public JobExecutionException GetException()
        {
            return new JobExecutionException(Message)
            {
                RefireImmediately = RefireImmediately,
                UnscheduleAllTriggers = UnscheduleAllTriggers,
                UnscheduleFiringTrigger = UnscheduleFiringTrigger
            };
        }
        
    }
}