using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constants.OrderCard
{
    /// <summary>
    /// 抽卡
    /// </summary>
    public class Order_Take:IOrderCardBase
    {
        public Order_Take()
        {
            Type = CardType.ORDERCARD;
            Name = OrderCardType.TAKE;
        }

        public int Type { get => Type; set => Type = value; }
        public int Name { get => Name; set => Name = value; }

        public void Effect()
        {
            throw new NotImplementedException();
            //TODO 抽卡
        }
    }
}
