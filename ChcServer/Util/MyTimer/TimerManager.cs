using ChcServer.Util.Concurrent;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace ChcServer.Util.MyTimer
{
    /// <summary>
    /// 定时器任务(计时器)管理类
    /// </summary>
    public class TimerManager
    {
        /// <summary>
        /// 单例模式
        /// </summary>
        private TimerManager instance = null;
        public TimerManager Instance {
            get
            {
                lock (instance)
                {
                    if (instance == null)
                    {
                        instance = new TimerManager();
                    }
                    return instance;
                }
            }
        }
        private Timer timer = null;

        /// <summary>
        /// 储存任务的线程安全字典 任务id 和 任务模型的映射
        /// </summary>
        private ConcurrentDictionary<int , TimerModle> idTimerDic = null;
        //private Dictionary<int , TimerModle> idTimerDic = null;
        /// <summary>
        /// 要移除的任务id列表
        /// </summary>
        private List<int> removelistId = null;

        /// <summary>
        /// 线程安全的任务ID 每添加一个任务 要求自增一次 保证任务ID的唯一性
        /// </summary>
        private ConcurrentInt concurrentID;

        public TimerManager()
        {
            concurrentID = new ConcurrentInt(-1);
            idTimerDic = new ConcurrentDictionary<int, TimerModle>();

            removelistId = new List<int>();
            timer = new Timer(1000);
            timer.Elapsed += Timer_Elapsed;//计时器达到时间间隔后触发的事件
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            TimerModle timerModle = null;
            //不是线程安全的 需要锁
            lock (removelistId)
            {
                foreach (var id in removelistId)
                {
                    //out类似于ref 相当于引用传递 out是传出 ref是传入
                    //ref参数在传入前需要初始化
                    //out参数在传入前不需要初始化
                    //out在调用函数里必须赋值，语法检查里能确保这个变量确实被函数修改了。
                    //out在调用函数前的赋值是被忽略的，也就是说与方法调用前的值无关，所以只要声明过就可以了。

                    idTimerDic.TryRemove(id, out timerModle);
                    //out确保timerModele被重新赋值了
                }
                removelistId.Clear();
            }

            //依次触发每个任务模型的操作
            foreach (var timemodle in idTimerDic.Values)
            {
                if(DateTime.Now.Ticks >= timerModle.Time)
                {
                    timemodle.Run();
                    //如果现在的时间已经大于等于任务模型规定执行的时间
                    //则触发任务模型执行的事件
                }  
            }
        }

        /// <summary>
        /// 外部设置延迟多少秒时间的任务 方法由外部指定
        /// </summary>
        /// <param name="id"></param>
        /// <param name="delaytime"></param>
        public void AddTimerEvent(long delaytime , TimerDG td)
        {
            long time = DateTime.Now.Ticks + delaytime;
            TimerModle timerModle = new TimerModle(concurrentID.Add_Get(), time, td);
            idTimerDic.TryAdd(timerModle.ID, timerModle);
        }

        /// <summary>
        /// 设置定时触发的任务
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="td"></param>
        public void AddTimerEvent(DateTime dateTime, TimerDG td)
        {
            long delaytime = dateTime.Ticks - DateTime.Now.Ticks;
            if(delaytime < 0)
            {
                return;
                //如果延迟时间小于0 证明定时日期在过去
            }
            TimerModle timerModle = new TimerModle(concurrentID.Add_Get(), delaytime, td);
            idTimerDic.TryAdd(timerModle.ID, timerModle);
        }
    }
}
