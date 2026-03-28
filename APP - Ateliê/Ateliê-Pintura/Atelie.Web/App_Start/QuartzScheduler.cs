using System;
using Atelie.Infrastructure.Scheduler;
using Ninject;
using Quartz;
using Quartz.Impl;

namespace Atelie.Web.App_Start
{
    public static class QuartzScheduler
    {
        public static async void Start(IKernel kernel)
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();

            IScheduler scheduler = await schedulerFactory.GetScheduler();

            scheduler.JobFactory = new NinjectJobFactory(kernel);

            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<GeneratePaymentsJob>()
                .WithIdentity("generatePaymentsJob", "faturamento")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("generatePaymentsTrigger", "faturamento")
                .WithCronSchedule("0 14 1 * * ?", x => x.WithMisfireHandlingInstructionFireAndProceed())
                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }
    }
}