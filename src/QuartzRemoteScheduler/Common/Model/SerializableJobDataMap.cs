using System.Collections.Generic;
using MessagePack;
using Quartz;

namespace QuartzRemoteScheduler.Common.Model
{
    [MessagePackObject(keyAsPropertyName: true)]
    internal class SerializableJobDataMap
    {

        public SerializableJobDataMapItem[] Values { get; set; }
        
        public SerializableJobDataMap(JobDataMap data)
        {
            var vals = new List<SerializableJobDataMapItem>(); 
            foreach (var dataKey in data.Keys)
            {
                
                var val = data[dataKey];
                if (val is string sv)
                {
                    vals.Add(new SerializableJobDataMapItem(){Key = dataKey, StringValue = sv});
                }

                if (val is int iv)
                {
                    vals.Add(new SerializableJobDataMapItem(){Key = dataKey, IntValue = iv});
                }

                if (val is long lv)
                {
                    vals.Add(new SerializableJobDataMapItem(){Key = dataKey, LongValue = lv});
                }

                if (val is bool bv)
                {
                    vals.Add(new SerializableJobDataMapItem(){Key = dataKey, BoolValue = bv});
                }
            }

            Values = vals.ToArray();
        }

        public SerializableJobDataMap()
        {
            
        }
        
        public JobDataMap GetMap()
        {
            IDictionary<string, object> o = new Dictionary<string, object>();
            foreach (var item in Values)
            {
                object d = item.StringValue;
                if (item.IntValue.HasValue)
                    d = d ?? item.IntValue.Value;
                if (item.LongValue.HasValue)
                    d = d ?? item.LongValue.Value;
                if (item.BoolValue.HasValue)
                    d = d ?? item.BoolValue.Value;
                o[item.Key] = d;
            }

            return new JobDataMap(o);
        }
    }
}