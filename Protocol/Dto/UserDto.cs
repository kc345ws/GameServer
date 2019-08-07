using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Protocol.Dto
{
    /// <summary>
    /// 角色数据传输对象
    /// </summary>
    /// 
    [Serializable]
    public class UserDto
    {
        public int ID;//角色ID
        public string Name;//角色名字
        public int Been;//豆子的数量

        public int WinCount;//胜场
        public int LoseCount;//负场
        public int RunCount;//逃跑场

        public int Lv;//等级
        public int Exp;//经验


        public UserDto(int id , string name,int benn,int win , int lose, int run, int lv , int exp)
        {
            ID = id;
            this.Name = name;
            this.Been = benn;
            this.WinCount = win;
            this.LoseCount = lose;
            this.RunCount = run;
            this.Lv = lv;
            this.Exp = exp;
        }

        public UserDto()
        {

        }
    }
}
