namespace Simple.Web.Test.Service
{
    public interface ITestService
    {
        string GetName();
    }

    public class TestService : ITestService
    {
        public string GetName()
        {
            return "Admin";
        }
    }
}