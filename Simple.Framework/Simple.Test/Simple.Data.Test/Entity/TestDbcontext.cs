using Microsoft.EntityFrameworkCore;
using Simple.EntityFrameworkCore;

namespace Simple.Dapper.Test
{
    public class TestDbcontext : AppDbContext
    {
        public TestDbcontext(string connectString) : base(connectString)
        {
        }

        public DbSet<SysUser> SysUser { get; set; }

        public DbSet<SysFunction> SysFunction { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}