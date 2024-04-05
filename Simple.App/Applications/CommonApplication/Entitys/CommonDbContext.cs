using Microsoft.EntityFrameworkCore;
using Simple.CommonApplication.Entitys;

using Simple.EntityFrameworkCore;

namespace Simple.CommonApplication
{
    public class CommonDbContext : AppDbContext
    {
        public CommonDbContext(DbContextOptions<CommonDbContext> options) : base(options)
        {
        }

        public DbSet<OperateLog> OperateLog { get; set; }
    }
}