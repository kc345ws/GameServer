using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.Modle;
using ChcServer.Util.Concurrent;
using ChcServer;
using GameServer.DataBase;

namespace GameServer.Cache
{
    /// <summary>
    /// 帐号数据缓冲区
    /// </summary>
    public class AccountCache
    {
        private static AccountCache instance = new AccountCache();
        public static AccountCache Instance
        {
            get
            {
                lock (instance)
                {
                    if(instance == null)
                    {
                        instance = new AccountCache();
                    }
                    return instance;
                }
            }
        }

        #region 帐号注册登陆
        /// <summary>
        /// 账号与账号数据模型
        /// </summary>
        private Dictionary<string, AccountModle> accDic;

        /// <summary>
        /// 每个在线帐号的唯一ID表示
        /// </summary>
        public ConcurrentInt ID { get; private set; }

        private AccountCache()
        {
            accDic = new Dictionary<string, AccountModle>();
            ID = new ConcurrentInt(-1);
            acc_ClientDic = new Dictionary<string, ClientPeer>();
            Client_accDic = new Dictionary<ClientPeer, string>();
        }

        /// <summary>
        /// 判断帐号是否已经存在
        /// </summary>
        public bool IsExist(string account)
        {
            if (!accDic.ContainsKey(account))
            {
                AccountModle accountModle = MysqlPeer.Instance.GetAccountModleByAcc(account);
                if(accountModle != null)
                {
                    accDic.Add(account, accountModle);
                }
            }
            return accDic.ContainsKey(account);
        }

        /// <summary>
        /// 创建帐号
        /// </summary>
        /// <param name="account">帐号</param>
        /// <param name="password">密码</param>
        public void Create(string account, string password)
        {
            AccountModle accountModle = new AccountModle(ID.Add_Get(), account, password);
            accDic.Add(account, accountModle);
            MysqlPeer.Instance.AddAccount(accountModle);
        }

        /// <summary>
        /// 判断帐号密码是否匹配
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool isMatch(string account , string password)
        {
            AccountModle accountModle = accDic[account];
            return accountModle.PassWord == password;
        }
        #endregion

        #region 玩家上下线

        /// <summary>
        /// 玩家与连接对象的双向映射
        /// </summary>
        private Dictionary<string, ClientPeer> acc_ClientDic;
        private Dictionary<ClientPeer, string> Client_accDic;

        /// <summary>
        /// 玩家是否在线
        /// </summary>
        /// <returns></returns>
        public bool IsOnline(string account)
        {
            return acc_ClientDic.ContainsKey(account);
        }

        public bool IsOnline(ClientPeer clientPeer)
        {
            return Client_accDic.ContainsKey(clientPeer);
        }

        /// <summary>
        /// 玩家上线
        /// </summary>
        /// <param name="account">帐号</param>
        /// <param name="clientPeer">客户端连接对象</param>
        public void Online(string account , ClientPeer clientPeer)
        {
            acc_ClientDic.Add(account, clientPeer);
            Client_accDic.Add(clientPeer, account);
        }

        /// <summary>
        /// 玩家下线
        /// </summary>
        /// <param name="account">帐号</param>
        public void OffLine(string account)
        {
            ClientPeer clientPeer = acc_ClientDic[account];
            acc_ClientDic.Remove(account);
            Client_accDic.Remove(clientPeer);
        }

        /// <summary>
        /// 玩家下线
        /// </summary>
        /// <param name="clientPeer">客户端连接对象</param>
        public void OffLine(ClientPeer clientPeer)
        {
            string account = Client_accDic[clientPeer];
            acc_ClientDic.Remove(account);
            Client_accDic.Remove(clientPeer);
        }

        /// <summary>
        /// 获取在线玩家的ID
        /// </summary>
        /// <param name="account">在线玩家帐号</param>
        /// <returns></returns>
        public int GetOnlineID(string account)
        {
            AccountModle accountModle = accDic[account];
            return accountModle.ID;
        }

        /// <summary>
        /// 获取在线玩家的ID
        /// </summary>
        /// <param name="clientPeer">客户端连接对象</param>
        /// <returns></returns>
        public int GetOnlineID(ClientPeer clientPeer)
        {
            string account = Client_accDic[clientPeer];
            AccountModle accountModle = accDic[account];
            return accountModle.ID;
        }

        public string GetAccount(ClientPeer clientPeer)
        {
            return Client_accDic[clientPeer];
        }
        #endregion
    }
}
