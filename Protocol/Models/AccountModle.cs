using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Protocol.Models
{
    /// <summary>
    /// 帐号模型
    /// </summary>
    /// 
    [Serializable]
    public class AccountModle
    {
        public string Account { get; set; }
        public string Password { get; set; }

        public AccountModle()
        {

        }

        public AccountModle(string account , string pwd)
        {
            Account = account;
            Password = pwd;
        }
    }
}
