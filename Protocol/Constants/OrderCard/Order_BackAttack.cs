using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constants.OrderCard
{
    /// <summary>
    /// 反击
    /// </summary>
    public class Order_BackAttack:IOrderCardBase
    {
        public Order_BackAttack()
        {
            Type = CardType.ORDERCARD;
            Name = OrderCardType.BACKATTACK;
        }

        public int Type { get => Type; set => Type = value; }
        public int Name { get => Name; set => Name = value; }

        public void Effect()
        {
            throw new NotImplementedException();
        }
    }
}
