using System.Linq;
using System.Threading.Tasks;
using QuartzRemoteScheduler.Client;
using QuartzRemoteScheduler.Test.Common;
using Xunit;

namespace QuartzRemoteScheduler.Test
{
    [Collection("BasicSchedulerCollection")]
    public class ConnectionTest:IClassFixture<BasicSchedulerFixture>
    {
        private readonly BasicSchedulerFixture _scheduler;

        public ConnectionTest(BasicSchedulerFixture scheduler)
        {
            _scheduler = scheduler;
        }


        [Fact]
        public async Task SimpleConnectAsync()
        {
            RemoteSchedulerFactory f = new RemoteSchedulerFactory(_scheduler.Conf);
            var sch = await f.GetScheduler();
            Assert.NotNull(sch);
            Assert.Equal(sch.SchedulerName, _scheduler.LocalScheduler.SchedulerName);
            Assert.Equal(sch.SchedulerInstanceId, _scheduler.LocalScheduler.SchedulerInstanceId);
            Assert.Equal(sch.InStandbyMode, _scheduler.LocalScheduler.InStandbyMode);
            Assert.Equal(sch.IsShutdown, _scheduler.LocalScheduler.IsShutdown);
            Assert.Equal(sch.IsStarted, _scheduler.LocalScheduler.IsStarted);
        }
        
        [Fact]
        public async Task SimpleConnect2Async()
        {
            RemoteSchedulerFactory f = new RemoteSchedulerFactory(_scheduler.Conf);
            var sch = (await f.GetAllSchedulers()).FirstOrDefault();
            Assert.NotNull(sch);
            Assert.Equal(sch.SchedulerName, _scheduler.LocalScheduler.SchedulerName);
            Assert.Equal(sch.SchedulerInstanceId, _scheduler.LocalScheduler.SchedulerInstanceId);
            Assert.Equal(sch.InStandbyMode, _scheduler.LocalScheduler.InStandbyMode);
            Assert.Equal(sch.IsShutdown, _scheduler.LocalScheduler.IsShutdown);
            Assert.Equal(sch.IsStarted, _scheduler.LocalScheduler.IsStarted);
        }
        
        [Fact]
        public async Task SimpleConnect3Async()
        {
            RemoteSchedulerFactory f = new RemoteSchedulerFactory(_scheduler.Conf);
            var sch = await f.GetScheduler(_scheduler.LocalScheduler.SchedulerName);
            Assert.NotNull(sch);
            Assert.Equal(sch.SchedulerName, _scheduler.LocalScheduler.SchedulerName);
            Assert.Equal(sch.SchedulerInstanceId, _scheduler.LocalScheduler.SchedulerInstanceId);
            Assert.Equal(sch.InStandbyMode, _scheduler.LocalScheduler.InStandbyMode);
            Assert.Equal(sch.IsShutdown, _scheduler.LocalScheduler.IsShutdown);
            Assert.Equal(sch.IsStarted, _scheduler.LocalScheduler.IsStarted);
        }

        [Fact]
        public async Task GetMetadataTestAsync()
        {
            RemoteSchedulerFactory f = new RemoteSchedulerFactory(_scheduler.Conf);
            var sch = await f.GetScheduler();
            var rem = await sch.GetMetaData();
            var loc = await _scheduler.LocalScheduler.GetMetaData();
            Assert.Equal(rem.Shutdown, loc.Shutdown);
            Assert.Equal(rem.Started, loc.Started);
            Assert.Equal(rem.Version, loc.Version);
            Assert.Equal(rem.RunningSince, loc.RunningSince);
            Assert.Equal(rem.SchedulerName, loc.SchedulerName);
            Assert.Equal(rem.SchedulerRemote, loc.SchedulerRemote);
            Assert.Equal(rem.SchedulerType, loc.SchedulerType);
            Assert.Equal(rem.InStandbyMode, loc.InStandbyMode);
            Assert.Equal(rem.JobStoreClustered, loc.JobStoreClustered);
            Assert.Equal(rem.JobStoreType, loc.JobStoreType);
            Assert.Equal(rem.SchedulerInstanceId, loc.SchedulerInstanceId);
            Assert.Equal(rem.ThreadPoolSize, loc.ThreadPoolSize);
            Assert.Equal(rem.ThreadPoolType, loc.ThreadPoolType);
            Assert.Equal(rem.JobStoreSupportsPersistence, loc.JobStoreSupportsPersistence);
            Assert.Equal(rem.NumberOfJobsExecuted, loc.NumberOfJobsExecuted);
        }

        [Fact]
        public async Task ClearTest()
        {
            RemoteSchedulerFactory f = new RemoteSchedulerFactory(_scheduler.Conf);
            var sch = await f.GetScheduler();
            await sch.Clear();
        }


    }
}
