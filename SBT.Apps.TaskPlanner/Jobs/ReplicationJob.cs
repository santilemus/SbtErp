using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBT.Apps.TaskPlanner.Jobs
{
    public class ReplicationJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler().ConfigureAwait(false);
            await scheduler.Start();

            JobDataMap datamap = context.JobDetail.JobDataMap; // de esta manera solo se recuperan los parametros del job
            //JobDataMap datamap = context.MergedJobDataMap;     // aqui se recuperan los parametros del job y los del trigger
            string sValue = datamap.GetString("Parametro0");   // ejemplo para exeder a los parametros del job
            var message = $"Tarea {sValue} ejecutada a las {DateTime.Now}";

            IJobDetail jobDetalle = JobBuilder.Create<ReplicationJob>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("Replication")
                //.WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(19, 00))
                .WithCronSchedule("0 0/1 * 1/1 * ? *")
                .StartAt(DateTime.UtcNow)
                .WithPriority(1)
                .Build();
            await scheduler.ScheduleJob(jobDetalle, trigger);
            Debug.WriteLine(message);
        }
    }
}
