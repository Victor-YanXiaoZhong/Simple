namespace Simple.Utils.Helper
{
    public class LockHelper
    {
        private const int LockeCount = 0x12fd; //素数，减少hash冲突，值越大冲突概率越小，但占用内存越大
        private static readonly int[] Locks = new int[LockeCount];

        public static event Action<string> MessageEv;

        /// <summary>以 关键字 锁运行方法 线程同步运行，排队执行</summary>
        /// <param name="key">关键字</param>
        /// <param name="act">方法体</param>
        /// <param name="timeOut">锁超时时间 默认0</param>
        public static void LockRun(string key, Action act, int timeOut = 0)
        {
            int index = (key.GetHashCode() & 0x7fffffff) % LockeCount;
            decimal timeCount = 0;
            //尝试0变1,进入对应index的临界状态;
            while (Interlocked.CompareExchange(ref Locks[index], 1, 0) == 1)
            {
                Thread.Sleep(1);
                ////可也以计数方式.每X次尝试失败则睡眠1,否则睡眠0
                timeCount += 0.01M;

                if (timeOut > 0 && timeCount > timeOut)
                {
                    MessageEv?.Invoke($"线程 {Thread.CurrentThread.ManagedThreadId}获取锁 【key = {key}】超时 耗时 {timeCount} s");
                    throw new FatalException($"线程 {Thread.CurrentThread.ManagedThreadId}获取锁 【key = {key}】超时 耗时 {timeCount} s");
                }
            }

            try
            {
                MessageEv?.Invoke($"线程 {Thread.CurrentThread.ManagedThreadId}已获取【{key}】锁，耗时 {timeCount} s，开始执行函数");
                act();
            }
            finally
            {
                Thread.VolatileWrite(ref Locks[index], 0);
                MessageEv?.Invoke($"线程 {Thread.CurrentThread.ManagedThreadId}【{key}】锁已释放");
            }
        }
    }
}