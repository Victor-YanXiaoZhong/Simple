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

    public class DefaultJob1 : IJobSchedule
    {
        public string JobName { get; set; } = "DefaultJob1";

        public void Execute()
        {
            Thread.Sleep(15000);
        }

        public void ScheduleConfig(Schedule schedule)
        {
            schedule.ToRunEvery(11).Seconds();
        }
    }

    public class DefaultJob2 : IJobSchedule
    {
        public string JobName { get; set; } = "DefaultJob2";

        public void Execute()
        {
            Thread.Sleep(13000);
        }

        public void ScheduleConfig(Schedule schedule)
        {
            schedule.ToRunEvery(12).Seconds();
        }
    }

    public class DefaultJob3 : IJobSchedule
    {
        public string JobName { get; set; } = "DefaultJob3";

        public void Execute()
        {
            Thread.Sleep(16000);
        }

        public void ScheduleConfig(Schedule schedule)
        {
            schedule.ToRunEvery(13).Seconds();
        }
    }
}