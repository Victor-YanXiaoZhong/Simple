using Simple.AdminApplication;

namespace Simple.CommonApplication.Application
{
    [Transient]
    public class AppInitService : IAppInitService
    {
        private AdminDbContext adminDb;

        public AppInitService(AdminDbContext adminDb)
        {
            this.adminDb = adminDb;
        }

        public ApiResult SysDbInit()
        {
            var result = ApiResult.Default;
            return result;
        }
    }
}