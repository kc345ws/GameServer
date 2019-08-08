using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.Fight
{
    [Serializable]
    public class ChatDto
    {
        public int UserID;
        public int ChatType;
        public string ChatText;

        public ChatDto() { }

        public ChatDto(int uid ,int type)
        {
            UserID = uid;
            ChatType = type;
        }

        public void Change(int uid , int type)
        {
            UserID = uid;
            ChatType = type;
        }

        public void SetText(string text)
        {
            ChatText = text;
        }
    }
}
