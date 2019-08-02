using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChcServer;
using Protocol.Code;
using Protocol.Dto;
using GameServer.Cache;
using ChcServer.Util.Concurrent;

namespace GameServer.Logic
{
    /// <summary>
    /// 帐号逻辑层模块
    /// </summary>
    public class AccountHandler : IHandler
    {
        ClientPeer clientPeer = null;

        
        /// <summary>
        /// 断开连接时玩家自动下线
        /// </summary>
        /// <param name="clientPeer"></param>
        public void OnDisConnect(ClientPeer clientPeer)
        {
            if (AccountCache.Instance.IsOnline(clientPeer))
            {
                AccountCache.Instance.OffLine(clientPeer);
            }
        }

        public void OnReceive(ClientPeer clientPeer, int subcode, object value)
        {
            this.clientPeer = clientPeer;
            AccountDto account = null;
            switch (subcode)
            {
                case AccountCode.LOGIN_CREQ:
                    account = value as AccountDto;
                    Console.WriteLine(account.Account);
                    Console.WriteLine(account.Password);
                    login(account.Account, account.Password);
                    break;

                case AccountCode.REGISTER_CREQ:
                    account = value as AccountDto;
                    Console.WriteLine(account.Account);
                    Console.WriteLine(account.Password);
                    register(account.Account, account.Password);
                    break;
            }
        }

        #region 注册帐号

        private void register(string account , string pwd)
        {
            //单线程执行防止并发操作
            SingleExecute.Instance.processSingle(()=>
            {
                //如果帐号存在
                if (AccountCache.Instance.IsExist(account))
                {
                    if (clientPeer != null)
                    {
                        clientPeer.StartSend(OpCode.ACCOUNT, AccountCode.REGISTER_SRES, "帐号已存在");
                    }
                    return;
                }
                else if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(pwd))
                {
                    if (clientPeer != null)
                    {
                        clientPeer.StartSend(OpCode.ACCOUNT, AccountCode.REGISTER_SRES, "帐号或密码不能为空");
                    }
                }
                else if (pwd.Length < 6 || pwd.Length > 16)
                {
                    if (clientPeer != null)
                    {
                        clientPeer.StartSend(OpCode.ACCOUNT, AccountCode.REGISTER_SRES, "密码不能小于6位或大于16位");
                    }
                }
                else
                {
                    AccountCache.Instance.Create(account, pwd);
                    clientPeer.StartSend(OpCode.ACCOUNT, AccountCode.REGISTER_SRES, "注册成功");
                }
            });  
        }

        #endregion

        #region 登陆帐号

        private void login(string acc , string pwd)
        {
            //单线程执行防止并发操作
            SingleExecute.Instance.processSingle(() =>
            {
                //如果帐号不存在
                if (!AccountCache.Instance.IsExist(acc))
                {
                    clientPeer.StartSend(OpCode.ACCOUNT, AccountCode.LOGIN_SRES, "帐号不存在");
                    return;
                }
                else if (!AccountCache.Instance.isMatch(acc, pwd))
                {
                    clientPeer.StartSend(OpCode.ACCOUNT, AccountCode.LOGIN_SRES, "密码不正确");
                    return;
                }
                else if (string.IsNullOrEmpty(acc) || string.IsNullOrEmpty(pwd))
                {
                    clientPeer.StartSend(OpCode.ACCOUNT, AccountCode.LOGIN_SRES, "帐号或密码不能为空");
                    return;
                }else if (AccountCache.Instance.IsOnline(clientPeer) || AccountCache.Instance.IsOnline(acc))
                {
                    clientPeer.StartSend(OpCode.ACCOUNT, AccountCode.LOGIN_SRES, "帐号已经在线");
                    return;
                }
                else
                {
                    AccountCache.Instance.Online(acc, clientPeer);
                    clientPeer.StartSend(OpCode.ACCOUNT, AccountCode.LOGIN_SRES, "登陆成功");
                }
            });
        }

        #endregion

    }
}
