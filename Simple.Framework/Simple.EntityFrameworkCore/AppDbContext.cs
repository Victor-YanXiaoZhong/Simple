using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Simple.Utils;
using Simple.Utils.Helper;

namespace Simple.EntityFrameworkCore
{
    public abstract class AppDbContext : DbContext
    {
        protected readonly string connectString;

        public AppDbContext(string connectString)
        {
            this.connectString = connectString;
        }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (ConfigHelper.GetValue<bool>("ShowEfCoreCommand"))
                optionsBuilder.UseLoggerFactory(new EFLoggerFactory());

            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>事务执行过程</summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public (bool, Exception?) RunWithTran(Action<IDbContextTransaction> func)
        {
            var trans = Database.BeginTransaction();
            try
            {
                func.Invoke(trans);
                trans.Commit();
                return (true, null);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                LogHelper.Error("事务执行出现异常", ex);
                return (false, ex);
            }
        }
    }
}