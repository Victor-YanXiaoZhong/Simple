using FluentScheduler;
using Simple.AdminApplication.Entitys;
using Simple.Job;
using Simple.Utils.Models.BO;

namespace Simple.AdminApplication.Jobs
{
    /// <summary>更新功能点的任务</summary>
    internal class PermissionUpdateJob : IJobSchedule
    {
        private AdminDbContext adminDb;

        public PermissionUpdateJob(AdminDbContext db)
        {
            this.adminDb = db;
        }

        public string JobName { get; set; } = "更新功能点数据信息任务";

        public void Execute()
        {
            var permissions = RedisHelper.StringGet<List<PermissionBO>>("permissions");
            if (permissions is null || permissions.Count == 0)
                return;

            var sysFunctions = adminDb.SysFunction.ToList();

            foreach (var function in sysFunctions)
            {
                var findPermission = permissions.FirstOrDefault(p => p.Sign == function.FunSign);

                ///找不到则禁用
                if (findPermission is null)
                {
                    function.Enabled = false;
                    adminDb.SysFunction.Update(function);
                }
                else//找到则更新
                {
                    function.FunGroup = findPermission.Group;
                    function.Name = findPermission.Name;
                    function.Description = findPermission.Description;
                    function.UpdateTime = DateTime.Now;
                }

                try
                {
                    adminDb.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            ///更新新增的权限
            foreach (var permission in permissions)
            {
                var entity = adminDb.SysFunction
                    .FirstOrDefault(p => p.FunSign == permission.Sign);

                if (entity == null)
                {
                    var data = new SysFunction
                    {
                        FunGroup = permission.Group,
                        FunSign = permission.Sign,
                        Name = permission.Name,
                        Description = permission.Description
                    };
                    adminDb.SysFunction.Add(data);
                    adminDb.SaveChanges();
                }

                try
                {
                    adminDb.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public void ScheduleConfig(Schedule schedule)
        {
            schedule.ToRunOnceIn(10);
        }
    }
}