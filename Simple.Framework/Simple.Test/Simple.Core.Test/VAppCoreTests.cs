using NUnit.Framework;
using Simple.Socket;
using Simple.Utils.Helper;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Simple.Utils.Test
{
    public class SimpleCoreTests
    {
        [SetUp]
        public void Setup()
        {
            ConfigHelper.Init(new string[] { "appsettings.json" }, false, true);
        }

        [Test]
        public void TcpLogTest()
        {
            for (int i = 0; i < 20; i++)
            {
                TcpLogClient.SendLog($"[TcpLogTest]，发送日志【{i}】当前线程 {Thread.CurrentThread.ManagedThreadId}");
            }

            Thread.Sleep(20 * 1000);
        }

        [Test]
        public void NoLockTest()
        {
            var tasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    TcpLogClient.SendLog($"[Nolock]当前线程 {Thread.CurrentThread.ManagedThreadId}");
                }));
            }
            Task.WaitAll(tasks.ToArray());
        }

        [Test]
        public void LockHelperTest()
        {
            var keyDic = new string[] { "a", "b" };
            var rand = new Random();
            var tasks = new List<Task>();

            LockHelper.MessageEv += msg =>
            {
                TcpLogClient.SendLog(msg);
            };
            int i = 0;
            while (i < 100)
            {
                Task.Run(() =>
                {
                    try
                    {
                        var key = $"ssss_{rand.Next(0, 5)}";
                        LockHelper.LockRun(key, act: () =>
                        {
                            Thread.Sleep(TimeSpan.FromSeconds(10));
                            TcpLogClient.SendLog($"[{key}]执行了一次函数，当前执行线程 {Thread.CurrentThread.ManagedThreadId}");
                        }, 5);
                    }
                    catch (Exception ex)
                    {
                        TcpLogClient.SendLog($"线程 {Thread.CurrentThread.ManagedThreadId}，[lock]异常 {ex.Message}");
                    }
                });
                Thread.Sleep(TimeSpan.FromSeconds(3));
                i++;
            }
        }

        [Test]
        public void ObjectTest()
        {
            var temp = new Temp { x = 1, y = "2", z = "哈哈", tim = DateTime.Now };
            TcpLogClient.SendLog("1: " + JsonHelper.ToJson(temp));
            temp.Next(s => s.x = 2);
            TcpLogClient.SendLog("2: " + JsonHelper.ToJson(temp));
            temp.Next(x => x.tim = DateTime.Now);
            TcpLogClient.SendLog("3: " + JsonHelper.ToJson(temp));

            Console.ReadLine();
        }

        [Test]
        public void SnowFlakeHelperTest()
        {
            var maxCount = 100000;
            long preId = 0;

            for (int i = 0; i < maxCount; i++)
            {
                var id = IdHelper.GetId();
                Assert.That(id > preId);
                preId = id;
                //LogHelper.Debug("Id记录:" + id.ToString());
            }

            var ids = new List<long>();
            Parallel.For(0, 10, i =>
            {
                for (var j = 0; j < maxCount; j++)
                {
                    var id = IdHelper.GetId();
                    Assert.That(id > 0);
                    lock (ids)
                    {
                        ids.Add(id);
                    }
                }
            });
            Assert.That(ids.Count == 10 * maxCount);
        }

        [Test]
        public void LogTest()
        {
            var ex = new Exception("测试的异常");
            LogHelper.Info("info", ex);
            LogHelper.Warn("Warn", ex);
            LogHelper.Debug("Debug", ex);
            LogHelper.Error("Error", ex);
            LogHelper.Fatal("Fatal", ex);
        }

        public class Temp
        {
            public int x { get; set; } = 1;
            public string y { get; set; } = "2";
            public string z { get; set; } = "哈哈";
            public DateTime tim { get; set; } = DateTime.Now;
        }
    }
}