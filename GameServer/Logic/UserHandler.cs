using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ChcServer;
using Protocol.Code;
using GameServer.Cache;
using GameServer.Model;
using Protocol.Dto;
using ChcServer.Util.Concurrent;

namespace GameServer.Logic
{
    /// <summary>
    /// 用户逻辑处理层
    /// </summary>
    public class UserHandler : IHandler
    {
        private static UserHandler instance = new UserHandler();
        public static UserHandler Instance
        {
            get
            {
                lock (instance)
                {
                    if (instance == null)
                    {
                        instance = new UserHandler();
                    }
                    return instance;
                }
            }
        }

        private UserHandler() { }

        public void OnDisConnect(ClientPeer client)
        {
            if (UserCache.Instance.IsOnline(client))
            {
                Console.WriteLine("玩家:"+client.Clientsocket.RemoteEndPoint+"的角色:" + UserCache.Instance.GetModelByClientPeer(client).Name + "下线");
                UserCache.Instance.Offline(client);              
            }
        }

        public void OnReceive(ClientPeer client, int subCode, object value)
        {
            switch (subCode)
            {
                case UserCode.CREATE_USER_CREQ:
                    //Console.WriteLine(client.Clientsocket.RemoteEndPoint + "要创建角色:" + value.ToString());
                    create(client, value.ToString());
                    break;
                case UserCode.GET_USER_CREQ:
                    getInfo(client);
                    break;
                case UserCode.LOGIN_USER_CREQ:
                    online(client);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="client">客户端的连接对象</param>
        /// <param name="name">客户端传过来的名字</param>
        private void create(ClientPeer client, string name)
        {
            SingleExecute.Instance.processSingle(
                ()=>
                {
                    //判读这个客户端是不是非法登录
                    if (!AccountCache.Instance.IsOnline(client))
                    {
                        client.StartSend(OpCode.USER, UserCode.CREATE_USER_SRES, -1);//"客户端非法登录"
                        return;
                    }
                    //获取账号id
                    int accountId = AccountCache.Instance.GetOnlineID(client);
                    //判断一下 这个账号以前有没有角色
                    if (UserCache.Instance.IsExist(accountId))
                    {
                        client.StartSend(OpCode.USER, UserCode.CREATE_USER_SRES, -2);//"已经有角色 不能重复创建"
                        return;
                    }
                    //没有问题 才可以创建
                    UserCache.Instance.Create(name, accountId);
                    client.StartSend(OpCode.USER, UserCode.CREATE_USER_SRES, 0);//"创建成功"
                    Console.WriteLine("玩家:" + client.Clientsocket.RemoteEndPoint + ",创建角色:" + name + "成功");
                }
           );
        }

        /// <summary>
        /// 获取角色的信息
        /// </summary>
        /// <param name="client"></param>
        private void getInfo(ClientPeer client)
        {
            SingleExecute.Instance.processSingle(
                () =>
                {
                      //判读这个客户端是不是非法登录
                      if (!AccountCache.Instance.IsOnline(client))
                      {
                          client.StartSend(OpCode.USER, UserCode.GET_USER_SRES, -1);//"客户端非法登录"
                          return;
                      }
                      int accountId = AccountCache.Instance.GetOnlineID(client);

                      if (UserCache.Instance.IsExist(accountId) == false)
                      {
                          client.StartSend(OpCode.USER, UserCode.GET_USER_SRES, -2);//"没有创建角色 不能获取信息"
                          return;
                      }
                      //代码执行到这里 就代表有角色
                      //自动上线角色
                      online(client);

                      //给客户端发送自己的角色信息
                      //通过账号ID获取角色信息
                      UserModel model = UserCache.Instance.GetModelByAccountId(accountId);
                      UserDto dto = new UserDto(model.Id,model.Name, model.Been, model.WinCount, model.LoseCount, model.RunCount, model.Lv, model.Exp);
                      client.StartSend(OpCode.USER, UserCode.GET_USER_SRES, dto);//"获取成功"
                    Console.WriteLine("玩家:" + client.Clientsocket.RemoteEndPoint + "的角色" + model.Name + "上线");
                  }
             );

        }

        /// <summary>
        /// 角色上线
        /// </summary>
        /// <param name="client"></param>
        private void online(ClientPeer client)
        {
            SingleExecute.Instance.processSingle(
                () =>
                {
                       //判读这个客户端是不是非法登录
                       if (!AccountCache.Instance.IsOnline(client))
                       {
                           client.StartSend(OpCode.USER, UserCode.LOGIN_USER_SRES, -1);//"客户端非法登录"
                           return;
                       }
                       int accountId = AccountCache.Instance.GetOnlineID(client);

                       if (UserCache.Instance.IsExist(accountId) == false)
                       {
                           client.StartSend(OpCode.USER, UserCode.LOGIN_USER_SRES, -2);//"没有角色 不能上线"
                           return;
                       }

                       //上线成功了
                       int userId = UserCache.Instance.GetId(accountId);
                       UserCache.Instance.Online(client, userId);
                       client.StartSend(OpCode.USER, UserCode.LOGIN_USER_SRES, 0);//"上线成功"
                   }
              );
        }

    }
}
