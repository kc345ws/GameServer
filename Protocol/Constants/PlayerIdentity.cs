using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constants
{
    public class PlayerIdentity
    {
        public const int FRAMER = 0;
        public const int LANDLORD = 1;

        public string GetName(int identity)
        {
            if(identity == FRAMER)
            {
                return "农民";
            }
            else
            {
                return "地主";
            }
        }
    }
}
