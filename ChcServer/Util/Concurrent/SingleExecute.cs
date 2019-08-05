using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace ChcServer.Util.Concurrent
{
    /// <summary>
    /// 单线程执行的委托
    /// </summary>
    public delegate void SingleDelegate();

    /// <summary>
    /// 单线程池
    /// </summary>

    public class SingleExecute
    {
        /// <summary>
        /// 互斥量
        /// </summary>
        public Mutex mutex = new Mutex();

        private static SingleExecute instance = new SingleExecute();
        public static SingleExecute Instance
        {
            get
            {
                lock (instance)
                {
                    if(instance == null)
                    {
                        instance = new SingleExecute();
                    }
                    return instance;
                }
            }
        }

        private SingleExecute()
        {
            mutex = new Mutex();
        }


        public void processSingle(SingleDelegate singleDelegate)
        {
            lock (this)
            {
                mutex.WaitOne();//申请锁
                singleDelegate();//执行外部操作
                mutex.ReleaseMutex();//释放锁
            }
        }
    }
}
