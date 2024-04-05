using FluentScheduler;

namespace Simple.Job
{
    /// <summary>继承此接口，自动实现任务调度，只能用无参构造函数</summary>
    public interface IJobSchedule : IJob
    {
        /// <summary>Job名称</summary>
        public string JobName { get; set; }

        /// <summary>配置调度器</summary>
        /// <param name="schedule"></param>
        void ScheduleConfig(Schedule schedule);
    }
}