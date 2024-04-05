using Microsoft.EntityFrameworkCore;
using Simple.AdminApplication.Entitys;

using Simple.EntityFrameworkCore;

namespace Simple.AdminApplication
{
    public class AdminDbContext : AppDbContext
    {
        public AdminDbContext(string connectstr) : base(connectstr)
        {
        }

        public AdminDbContext(DbContextOptions<AdminDbContext>? options) : base(options)
        {
        }

        public DbSet<SysDicType> SysDicType { get; set; }
        public DbSet<SysDicValue> SysDicValue { get; set; }
        public DbSet<SysFunction> SysFunction { get; set; }
        public DbSet<SysMenu> SysMenu { get; set; }
        public DbSet<SysMenuFunction> SysMenuFunction { get; set; }
        public DbSet<SysRole> SysRole { get; set; }
        public DbSet<SysRoleMenu> SysRoleMenu { get; set; }
        public DbSet<SysRoleFunction> SysRoleFunction { get; set; }
        public DbSet<SysUser> SysUser { get; set; }
        public DbSet<SysOrgnization> SysOrgnization { get; set; }
        public DbSet<SysJobLog> SysJobLog { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // EF 6多对多flutapi配置 https://docs.microsoft.com/zh-cn/ef/core/modeling/relationships?tabs=data-annotations-simple-key%2Csimple-key%2Cfluent-api
            modelBuilder.Entity<SysMenu>()
                .HasMany(p => p.SysFunctions)
                .WithMany(p => p.SysMenus)
                .UsingEntity<SysMenuFunction>(
                    x => x.HasOne(px => px.SysFunction)
                        .WithMany(px => px.SysMenuFunctions)
                        .HasForeignKey(px => px.SysFunctionId),
                    x => x.HasOne(px => px.SysMenu)
                        .WithMany(px => px.SysMenuFunctions)
                        .HasForeignKey(px => px.SysMenuId),
                    x => x.HasKey(px => new { px.SysMenuId, px.SysFunctionId }));

            modelBuilder.Entity<SysRole>()
                .HasMany(p => p.SysMenus)
                .WithMany(p => p.SysRoles)
                .UsingEntity<SysRoleMenu>(
                    x => x.HasOne(t => t.SysMenu)
                        .WithMany(t => t.SysRoleMenus)
                        .HasForeignKey(t => t.SysMenuId),
                    x => x.HasOne(t => t.SysRole)
                        .WithMany(t => t.SysRoleMenus)
                        .HasForeignKey(t => t.SysRoleId),
                    x => x.HasKey(t => new { t.SysRoleId, t.SysMenuId })
                );
        }
    }
}