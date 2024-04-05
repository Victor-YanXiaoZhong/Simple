// See https://aka.ms/new-console-template for more information
using Simple.Utils;
using Simple.Utils.Helper;

ConfigHelper.Init(new string[] { "appsettings.json" }, false, true);

LockHelperTest();

static void LockHelperTest()
{
    var keyDic = new string[] { "a", "b" };
    var rand = new Random();
    var tasks = new List<Task>();

    LockHelper.MessageEv += msg =>
    {
        ConsoleHelper.Info(msg);
    };
    var i = 0;
    while (true)
    {
        Task.Run(() =>
        {
            try
            {
                var key = $"ssss_001";
                ConsoleHelper.Info($"线程 {Thread.CurrentThread.ManagedThreadId}开始请求锁");
                LockHelper.LockRun(key, act: () =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    ConsoleHelper.Info($"线程 {Thread.CurrentThread.ManagedThreadId} [{key}]执行了一次函数");
                }, 5);
            }
            catch (Exception ex)
            {
                ConsoleHelper.Err(ex.Message);
            }
        });
        i++;
        Thread.Sleep(TimeSpan.FromSeconds(3));
    }
    Console.ReadLine();
}