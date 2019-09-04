using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constants.OrderCard
{
    public class Order_Attack:IOrderCardBase
    {
        public Order_Attack()
        {
            Type = CardType.ORDERCARD;
            Name = OrderCardType.ATTACK;
        }

        //public int Type { get => Type; set => Type = value; }
       // public int Name { get => Name; set => Name = value; }

        public void Effect()
        {
            throw new NotImplementedException();
            //TODO 攻击
        }
    }
}
