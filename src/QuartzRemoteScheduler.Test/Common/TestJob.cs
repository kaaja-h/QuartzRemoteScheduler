using System.Threading.Tasks;
using Quartz;

namespace QuartzRemoteScheduler.Test.Common
{
    public class TestJob:IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            context.MergedJobDataMap["executed"] = true;
            return Task.CompletedTask;
            
        }
    }
}