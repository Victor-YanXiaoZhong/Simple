using MediatR;
using Simple.AdminApplication.Entitys;

namespace Simple.AdminApplication.Events
{
    public class JobLogStorageHandle : INotificationHandler<AppDomainEvent<SysJobLog>>
    {
        private AdminDbContext db;

        public JobLogStorageHandle(AdminDbContext db)
        {
            this.db = db;
        }

        public Task Handle(AppDomainEvent<SysJobLog> jobInfo, CancellationToken cancellationToken)
        {
            db.SysJobLog.Add(jobInfo.Value);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogHelper.Error("保存定时任务日志异常", ex);
            }
            return Task.CompletedTask;
        }
    }
}