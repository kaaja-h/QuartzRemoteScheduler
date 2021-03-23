using System;
using Microsoft.VisualStudio.Threading;
using Quartz;
using QuartzRemoteScheduler.Common.Model;

namespace QuartzRemoteScheduler.Client.Model
{
    
    internal class RemoteTrigger : ITrigger
    {

        private readonly Connector _connector;
        private JoinableTaskFactory _jtf = new JoinableTaskFactory(new JoinableTaskContext());
        
        public int CompareTo(ITrigger other)
        {
            if ((other == null || other.Key == null) && this.Key == null)
                return 0;
            if (other == null || other.Key == null)
                return -1;
            return this.Key == null ? 1 : this.Key.CompareTo(other.Key);
        }

        public TriggerBuilder GetTriggerBuilder()
        {
            throw new NotImplementedException();
        }

        public IScheduleBuilder GetScheduleBuilder()
        {
            throw new NotImplementedException();
        }

        public bool GetMayFireAgain()
        {
            return _jtf.Run(() => _connector.TriggerRpc.GetMayFireAgainAsync(new SerializableTriggerKey(Key)));
        }

        public DateTimeOffset? GetNextFireTimeUtc()
        {
            return _jtf.Run(() => _connector.TriggerRpc.GetNextFireTimeUtcAsync(new SerializableTriggerKey(Key)));
        }

        public DateTimeOffset? GetPreviousFireTimeUtc()
        {
            return _jtf.Run(() => _connector.TriggerRpc.GetPreviousFireTimeUtcAsync(new SerializableTriggerKey(Key)));
            
        }

        public DateTimeOffset? GetFireTimeAfter(DateTimeOffset? afterTime)
        {
            return _jtf.Run(() => _connector.TriggerRpc.GetFireTimeAfterAsync(new SerializableTriggerKey(Key),afterTime));
        }

        public ITrigger Clone()
        {
            throw new NotImplementedException();
        }

        public TriggerKey Key { get; set; }
        public JobKey JobKey { get; set; }
        public string Description { get; set; }
        public string CalendarName { get; set; }
        public JobDataMap JobDataMap { get; set; }
        public DateTimeOffset? FinalFireTimeUtc { get; set; }
        public int MisfireInstruction { get; set; }
        public DateTimeOffset? EndTimeUtc { get; set; }
        public DateTimeOffset StartTimeUtc { get; set; }
        public int Priority { get; set; }
        public bool HasMillisecondPrecision { get; set; }
        
        public RemoteTrigger(SerializableTrigger data, Connector connector)
        {
            _connector = connector;
            Key = data.Key.ToKey();
            JobKey = data.JobKey.ToKey();
            Description = data.Description;
            CalendarName = data.CalendarName;
            JobDataMap = data.JobDataMap?.GetMap();
            FinalFireTimeUtc = data.FinalFireTimeUtc;
            
            MisfireInstruction = data.MisfireInstruction;
            EndTimeUtc = data.EndTimeUtc;
            StartTimeUtc = data.StartTimeUtc;
            Priority = data.Priority;
            HasMillisecondPrecision = data.HasMillisecondPrecision;
        }
    }
}