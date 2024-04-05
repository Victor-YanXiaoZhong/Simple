using FluentScheduler;
using Simple.JobManageConsole.Help;
using Simple.WinUI.Forms;
using Simple.WinUI.Helper;
using System.ComponentModel;

namespace Simple.JobManageConsole
{
    public partial class JobManageConsoleForm : BaseForm
    {
        private StorageHelper<List<ScheduleVO>> storage = new StorageHelper<List<ScheduleVO>>("Schedule");

        public JobManageConsoleForm()
        {
            InitializeComponent();
        }

        private Schedule currentSchedule
        {
            get
            {
                if (gdvSchedule.CurrentRow is null) return null;
                return (Schedule)gdvSchedule.CurrentRow.DataBoundItem;
            }
        }

        private List<Schedule> Schedules { get; set; } = new List<Schedule>();

        private void Jobmanager_JobStart(JobStartInfo info)
        {
            var msg = $"{info.StartTime}，任务[{info.Name}]已开始执行";
            ShowMessage(msg);
            LogHelper.Info(msg, info.Name);
        }

        private void Jobmanager_JobEnd(JobEndInfo info)
        {
            var msg = $"{DateTime.Now}，任务[{info.Name}]已执行完成，开始时间:{info.StartTime}，执行时间:{info.Duration.Hours}时{info.Duration.Minutes}分{info.Duration.Seconds}秒，下次执行时间:{info.NextRun}";
            ShowMessage(msg);
            LogHelper.Info(msg, info.Name);
            GetJobs();
        }

        private void ShowMessage(string message)
        {
            this.SafeInvoke(new Action(() =>
            {
                txt_JobMessage.AppendText(Environment.NewLine);
                txt_JobMessage.AppendText(message);
            }));
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            GetJobs();
        }

        private void GetJobs()
        {
            Schedules = JobManager.AllSchedules.OrderBy(x => x.Name).ToList();
            BindingList<Schedule> schedules = new BindingList<Schedule>(Schedules);
            scheduleVOBindingSource.DataSource = schedules;
        }

        private void btnRunNow_Click(object sender, EventArgs e)
        {
            if (currentSchedule == null)
            {
                this.AlertToast("未选中有效调度");
                return;
            }
            if (this.Confirm($"确定要立即执行调度【{currentSchedule.Name}】吗？"))
            {
                currentSchedule.ToRunNow();
            }
        }

        private void btnDisabled_Click(object sender, EventArgs e)
        {
            if (currentSchedule == null)
            {
                this.AlertToast("未选中有效调度");
                return;
            }
            if (this.Confirm($"确定要停止执行调度【{currentSchedule.Name}】吗？"))
            {
                currentSchedule.Disable();
            }
        }

        private void btnEnable_Click(object sender, EventArgs e)
        {
            if (currentSchedule == null)
            {
                this.AlertToast("未选中有效调度");
                return;
            }
            if (this.Confirm($"确定要启用调度【{currentSchedule.Name}】吗？"))
            {
                currentSchedule.Enable();
            }
        }

        private void JobManageConsoleForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            storage.Data = Schedules.Select(x => new ScheduleVO { Name = x.Name, Disabled = x.Disabled, NextRun = x.NextRun }).ToList();
            storage.Save();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            UIHelper.RunWithLoading(this, "调度初始化中", action: () =>
            {
                var jobmanager = new JobSchedule();
                jobmanager.JobStart += Jobmanager_JobStart; ;
                jobmanager.JobEnd += Jobmanager_JobEnd;

                GetJobs();

                if (storage.Data.Count == 0)
                {
                    storage.Data = Schedules.Select(x => new ScheduleVO { Name = x.Name, Disabled = x.Disabled, NextRun = x.NextRun }).ToList();
                    storage.Save();
                }

                foreach (var schedule in Schedules)
                {
                    var tmp = storage.Data.FirstOrDefault(x => x.Name == schedule.Name);

                    if (tmp is null) continue;

                    if (tmp.Disabled && !schedule.Disabled)
                    {
                        schedule.Disable();
                    }
                }

                jobmanager.Start();
            });
        }
    }

    public class ScheduleVO
    {
        public string Name { get; set; }
        public bool Disabled { get; set; }
        public DateTime? NextRun { get; set; }
    }
}