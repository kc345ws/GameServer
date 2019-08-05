using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ChcServer.Util.MyTimer
{
    public delegate void TimerDG();//定时器任务的委托

    /// <summary>
    /// 定时器任务的数据模型
    /// </summary>
    public class TimerModle
    {
        /// <summary>
        /// 每个任务的唯一ID
        /// </summary>
        public int ID { get; }

        /// <summary>
        /// 定时器任务的时间
        /// </summary>
        public long Time { get; }

        /// <summary>
        /// 定时器到时后触发的事件
        /// </summary>
        private event TimerDG TimerEvent;

        public TimerModle(int id , long time , TimerDG td)
        {
            ID = id;
            Time = time;
            TimerEvent += td;
        }

        /// <summary>
        /// 触发定时器任务事件
        /// </summary>
        public void Run()
        {
            TimerEvent();
        }
    }
}
