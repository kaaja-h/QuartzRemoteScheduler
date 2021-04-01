using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Quartz;
using QuartzRemoteScheduler.Common;
using QuartzRemoteScheduler.Common.Model;

namespace QuartzRemoteScheduler.Server.Listeners
{

    internal abstract class EventListenerBase<T>
    {

        private readonly ConcurrentDictionary<T,bool> _listeners = new();
        
        public void Subscribe(T listener)
        {
            _listeners[listener] = true;
        }

        public void Unsubscribe(T listener)
        {
            _listeners.TryRemove(listener, out _);
        }

        protected async Task<IEnumerable<TRes>> RunFunctionOnListenersAsync<TRes>(Func<T, Task<TRes>> func)
        {
            var t = _listeners.Keys.Select(d => Catch(d,func));
            return await Task.WhenAll(t);
        }

        private async Task<TRes> Catch<TRes>(T data, Func<T, Task<TRes>> func)
        {
            try
            {
                return await func(data);
            }
            catch (Exception)
            {
                
            }
            return default;
        }

        private async Task Catch(T data, Func<T, Task> act)
        {
            try
            {
                await act(data);
            }
            catch (Exception)
            {
                
            }
        }

        protected async Task RunActionOnListenersAsync(Func<T,Task> act)
        {
            var t = _listeners.Keys.Select(d => Catch(d,act));
            await Task.WhenAll(t);
        }
        
        
        
        
    }
    
    internal class EventSchedulerListener:EventListenerBase<IRemoteSchedulerListener>, ISchedulerListener
    {
        private readonly IScheduler _scheduler;
        public event EventHandler<SchedulerBasicData> BasicDataChanged;
        
        

        public EventSchedulerListener(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        

        private void TriggerBasicData()
        {
            try
            {
                if (BasicDataChanged != null)
                {
                    var data = new SchedulerBasicData(_scheduler);
                    BasicDataChanged?.Invoke(this, data);
                }
            }
            catch (Exception )
            {
                //TODO
            }
        }
        
        
        public async Task JobScheduled(ITrigger trigger, CancellationToken cancellationToken = new CancellationToken())
        {
            var serializableTrigger = new SerializableTrigger(trigger);
            await RunActionOnListenersAsync(d => d.JobScheduledAsync(serializableTrigger, cancellationToken));
        }

        public async Task JobUnscheduled(TriggerKey triggerKey, CancellationToken cancellationToken = new CancellationToken())
        {
            SerializableTriggerKey key = triggerKey; 
            await RunActionOnListenersAsync(d => d.JobUnscheduledAsync(key,cancellationToken));

        }

        public async Task TriggerFinalized(ITrigger trigger, CancellationToken cancellationToken = new CancellationToken())
        {
            
        }

        public async Task TriggerPaused(TriggerKey triggerKey, CancellationToken cancellationToken = new CancellationToken())
        {
            
        }

        public async Task TriggersPaused(string triggerGroup, CancellationToken cancellationToken = new CancellationToken())
        {
            
        }

        public async Task TriggerResumed(TriggerKey triggerKey, CancellationToken cancellationToken = new CancellationToken())
        {
            
        }

        public Task TriggersResumed(string triggerGroup, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public Task JobAdded(IJobDetail jobDetail, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public Task JobDeleted(JobKey jobKey, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public Task JobPaused(JobKey jobKey, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public Task JobInterrupted(JobKey jobKey, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public Task JobsPaused(string jobGroup, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public Task JobResumed(JobKey jobKey, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public Task JobsResumed(string jobGroup, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public Task SchedulerError(string msg, SchedulerException cause,
            CancellationToken cancellationToken = new CancellationToken())
        {
            TriggerBasicData();
            return Task.CompletedTask;

        }

        public Task SchedulerInStandbyMode(CancellationToken cancellationToken = new CancellationToken())
        {
            TriggerBasicData();
            return Task.CompletedTask;
        }

        public  Task SchedulerStarted(CancellationToken cancellationToken = new CancellationToken())
        {
            TriggerBasicData();
            return Task.CompletedTask;

        }

        public Task SchedulerStarting(CancellationToken cancellationToken = new CancellationToken())
        {
            TriggerBasicData();
            return Task.CompletedTask;

        }

        public Task SchedulerShutdown(CancellationToken cancellationToken = new CancellationToken())
        {
            TriggerBasicData();
            return Task.CompletedTask;
        }

        public Task SchedulerShuttingdown(CancellationToken cancellationToken = new CancellationToken())
        {
            TriggerBasicData();
            return Task.CompletedTask;
        }

        public Task SchedulingDataCleared(CancellationToken cancellationToken = new CancellationToken())
        {
            TriggerBasicData();
            return Task.CompletedTask;
        }
    }
}