using FluentScheduler;
using Simple.Job;

namespace Simple.AdminApplication.Jobs
{
    internal class TestJob : IJobSchedule
    {
        public string JobName { get; set; } = "TestJob";

        public void ScheduleConfig(Schedule schedule)
        {
            schedule.ToRunEvery(5).Minutes();
        }

        public void Execute()
        {
            Console.WriteLine($"{DateTime.Now} 执行了一次测试任务");
        }
    }
}