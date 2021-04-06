using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        protected async Task<IEnumerable<TRes>> RunActionOnListenersAsync<TRes>(Func<T, Task<TRes>> func)
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
}