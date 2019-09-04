using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constants.OrderCard
{
    /// <summary>
    /// 修养卡
    /// </summary>
    public class Order_Rest:IOrderCardBase
    {
        public Order_Rest()
        {
            Type = CardType.ORDERCARD;
            Name = OrderCardType.REST;
        }

        //public int Type { get => Type; set => Type = value; }
        //public int Name { get => Name; set => Name = value; }

        public void Effect()
        {
            throw new NotImplementedException();
            //TODO 修养
        }
    }
}
