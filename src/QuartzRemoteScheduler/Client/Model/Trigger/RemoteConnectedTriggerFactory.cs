using Quartz;
using QuartzRemoteScheduler.Common.Model;

namespace QuartzRemoteScheduler.Client.Model.Trigger
{
    static class RemoteConnectedTriggerFactory
    {
        public static ITrigger GetTrigger(SerializableTrigger data, Connector connector)
        {
            if (data.SerializableCronTrigger != null)
                return new RemoteConnectedCronTrigger(data.SerializableCronTrigger, connector);
            if (data.SerializableSimpleTrigger != null)
                return new RemoteConnectedSimpleTrigger(data.SerializableSimpleTrigger, connector);

            return new RemoteConnectedTrigger(data.SerializableTriggerBase, connector);
        }
    }
}