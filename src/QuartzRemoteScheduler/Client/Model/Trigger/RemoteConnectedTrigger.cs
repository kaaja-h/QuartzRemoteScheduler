using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Threading;
using Quartz;
using QuartzRemoteScheduler.Common.Model;

namespace QuartzRemoteScheduler.Client.Model.Trigger
{
    internal class RemoteConnectedTrigger:ITrigger
    {
        protected readonly Connector Connector;
        

        protected T RunSync<T>(Func<Task<T>> f)
        {
            var jtf = new JoinableTaskFactory(new JoinableTaskContext());
            return jtf.Run(f);
        } 
        
        protected void RunSync(Func<Task> f)
        {
            var jtf = new JoinableTaskFactory(new JoinableTaskContext());
            jtf.Run(f);
        } 
        
        
        public int CompareTo(ITrigger other)
        {
            if ((other == null || other.Key == null) && this.Key == null)
                return 0;
            if (other == null || other.Key == null)
                return -1;
            return this.Key == null ? 1 : this.Key.CompareTo(other.Key);
        }

        public virtual TriggerBuilder GetTriggerBuilder()
        {

            var tb = TriggerBuilder.Create()
                .EndAt(EndTimeUtc)
                .ForJob(JobKey)
                .StartAt(StartTimeUtc)
                .WithDescription(Description)
                .WithIdentity(Key)
                .WithPriority(Priority)
                .WithSchedule(GetScheduleBuilder())
                .UsingJobData(JobDataMap);
            if (!String.IsNullOrEmpty(CalendarName))
                tb.ModifiedByCalendar(CalendarName);
            return tb;
        }

        public virtual IScheduleBuilder GetScheduleBuilder()
        {
            throw new NotImplementedException();
        }

        public bool GetMayFireAgain()
        {
            return RunSync(
               ()=> Connector.TriggerRpc.GetMayFireAgainAsync(Key)
            );
        }

        public DateTimeOffset? GetNextFireTimeUtc()
        {
            return RunSync(
                ()=> Connector.TriggerRpc.GetNextFireTimeUtcAsync(Key)
            );
        }

        public DateTimeOffset? GetPreviousFireTimeUtc()
        {
            return RunSync(
                ()=> Connector.TriggerRpc.GetPreviousFireTimeUtcAsync(Key)
            );
        }

        public DateTimeOffset? GetFireTimeAfter(DateTimeOffset? afterTime)
        {
            return RunSync(
                ()=> Connector.TriggerRpc.GetFireTimeAfterAsync(Key,afterTime)
            );
        }

        public ITrigger Clone()
        {
            return GetTriggerBuilder().Build();
        }

        public int Priority { get; set; }

        public TriggerKey Key { get;  }
        public JobKey JobKey { get;  }
        public string Description { get;  }
        public string CalendarName { get;  }
        public JobDataMap JobDataMap { get;  }
        public DateTimeOffset? FinalFireTimeUtc { get;  }
        public int MisfireInstruction { get;  }
        public DateTimeOffset? EndTimeUtc { get;  }
        public DateTimeOffset StartTimeUtc { get;  }
        
        public bool HasMillisecondPrecision { get;  }


        public RemoteConnectedTrigger(SerializableTriggerBase data, Connector connector)
        {
            this.Connector = connector;

            Key = data.Key;
            JobKey = data.JobKey;
            Description = data.Description;
            CalendarName = data.CalendarName;
            JobDataMap = data.JobDataMap.GetMap();
            FinalFireTimeUtc = data.FinalFireTimeUtc;
            MisfireInstruction = data.MisfireInstruction;
            EndTimeUtc = data.EndTimeUtc;
            StartTimeUtc = data.StartTimeUtc;
            HasMillisecondPrecision = data.HasMillisecondPrecision;
            Priority = data.Priority;
        }
    }
}