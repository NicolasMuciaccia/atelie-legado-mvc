using Ninject;
using Quartz;
using Quartz.Spi;

namespace Atelie.Web.App_Start
{
    public class NinjectJobFactory : IJobFactory
    {
        private readonly IKernel _kernel;

        public NinjectJobFactory(IKernel kernel)
        {
            _kernel = kernel;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return (IJob)_kernel.Get(bundle.JobDetail.JobType);
        }

        public void ReturnJob(IJob job)
        {
            _kernel.Release(job);
        }
    }
}