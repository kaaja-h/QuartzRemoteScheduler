using MessagePack;
using Quartz.Util;

namespace QuartzRemoteScheduler.Common.Model
{
    [MessagePackObject(keyAsPropertyName: true)]
    abstract class KeyData<T>where T:Key<T>
    {
        
        public string Name { get; set; }
        public string Group { get; set; }

        protected KeyData()
        {
            
        }

        protected KeyData(T key)
        {
            Name = key.Name;
            Group = key.Group;
        }

        public abstract T ToKey();

    }
}