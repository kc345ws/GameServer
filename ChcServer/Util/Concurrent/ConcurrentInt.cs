using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChcServer.Util.Concurrent
{
    /// <summary>
    /// 线程安全的int类型
    /// </summary>
    public class ConcurrentInt
    {
        private int value;
        public ConcurrentInt(int value)
        {
            this.value = value;
        }

        /// <summary>
        /// 线程安全的自增并获取值
        /// </summary>
        /// <returns></returns>
        public int Add_Get()
        {
            lock (this)
            {
                value++;
                return value;
            }
        }

        /// <summary>
        /// 线程安全的自减并返回值
        /// </summary>
        /// <returns></returns>
        public int Reduce_Get()
        {
            lock (this)
            {
                value--;
                return value;
            }
        }

        /// <summary>
        /// 返回值
        /// </summary>
        /// <returns></returns>
        public int Get()
        {
            return value;
        }
    }
}
