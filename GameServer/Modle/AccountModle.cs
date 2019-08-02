﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Modle
{
    /// <summary>
    /// 帐号数据模型
    /// </summary>
    public class AccountModle
    {
        public int ID { get; }
        public string Account { get; }
        public string PassWord { get; set; }

        public AccountModle()
        {

        }

        public AccountModle(int id , string account ,string pwd)
        {
            ID = id;
            Account = account;
            PassWord = pwd;
        }
    }
}