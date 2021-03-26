using System.Linq;
using System.Text.RegularExpressions;
using MessagePack;
using Quartz.Impl.Matchers;
using Quartz.Util;
using QuartzRemoteScheduler.Client.Model.Trigger;

namespace QuartzRemoteScheduler.Common.Model
{
    [MessagePackObject(true)]
    class SerializableMatcher<T> where T:Key<T>
    {
        public string OperatorName { get; set; }
        public string StringValue { get; set; }

        private static (StringOperator op, string name)[] Names = new[]
        {
            (StringOperator.Anything, "Anything"), (StringOperator.Contains, "Contains"),
            (StringOperator.Equality, "Equality"),
            (StringOperator.EndsWith, "EndsWith"), (StringOperator.StartsWith, "StartsWith")
        };

        public SerializableMatcher()
        {
            
        }
        
        public SerializableMatcher(GroupMatcher<T> data)
        {
            OperatorName = Names.Where(d => d.op.Equals(data.CompareWithOperator)).Select(d => d.name).First();
            StringValue = data.CompareToValue;
        }
        
        public GroupMatcher<T> GetMatcher()
        {
            
            if (OperatorName == "Anything")
                return GroupMatcher<T>.AnyGroup();
            if (OperatorName == "Contains")
                return GroupMatcher<T>.GroupContains(StringValue);
            if (OperatorName == "Equality")
                return GroupMatcher<T>.GroupEquals(StringValue);
            if (OperatorName == "EndsWith")
                return GroupMatcher<T>.GroupEndsWith(StringValue);
            if (OperatorName == "StartsWith")
                return GroupMatcher<T>.GroupStartsWith(StringValue);
            return null;
        }
    }
}