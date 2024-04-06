using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.JobManageConsole.Jobs
{
    public class DefaultJob : IJobSchedule
    {
        public string JobName { get; set; } = "DefaultJob";

        public void Execute()
        {
            Thread.Sleep(1000);
        }

        public void ScheduleConfig(Schedule schedule)
        {
            schedule.ToRunEvery(10).Seconds();
        }
    }
}