using Quartz;
using Quartz.Impl;
using System;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;
using Quartz.Logging;

namespace SBT.Apps.TaskPlanner
{
    class Program
    {
        static async Task Main(string[] args)
        {
            LogProvider.SetCurrentLogProvider(new ServiceLogger());
            // Grab the Scheduler instance from the Factory
            StdSchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = await factory.GetScheduler();

            // and start it off
            await scheduler.Start();

            // some sleep to show what's happening
            await Task.Delay(TimeSpan.FromSeconds(10));

            // and last shut down the scheduler when you are ready to close your program
            await scheduler.Shutdown();
        }
    }
}
