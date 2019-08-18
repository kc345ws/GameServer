using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Modle;
using MySql.Data.MySqlClient;
using MySql.Data.Common;
using GameServer.Cache;
using GameServer.Model;

namespace GameServer.DataBase
{
    public class MysqlPeer
    {
        private static MysqlPeer instance = new MysqlPeer();
        public static MysqlPeer Instance
        {
            get
            {
                lock (instance)
                {
                    if(instance == null)
                    {
                        instance = new MysqlPeer();
                    }
                    return instance;
                }
            }
        }

        public MySqlConnection SqlConnection { get;private set; } = null;

        private MysqlPeer()
        {
            SqlConnection = new MySqlConnection("Database=gamedb;Data Source=localhost;User Id=root;Password=qhw739t");
        }
        /// <summary>
        /// 打开数据库连接
        /// </summary>
        private bool OpenConnection()
        {
            try
            {
                SqlConnection.Open();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
                //throw;
            }
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        private bool CloseConnection()
        {
            try
            {
                SqlConnection.Close();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
                //throw;
            }
        }

        /// <summary>
        /// 增加账号数据
        /// </summary>
        /// <param name="data"></param>
        public void AddAccount(AccountModle accountModle)
        {
            string query = string.Format("insert into account (account,password) values('{0}','{1}');", accountModle.Account, accountModle.PassWord);
            if (this.OpenConnection())
            {
                //如果数据库打开成功
                MySqlCommand sqlCommand = new MySqlCommand(query, SqlConnection);

                sqlCommand.ExecuteNonQuery();

                CloseConnection();
            }
        }

        public bool IsAccontExist(string account)
        {
            string query = string.Format("select * from account where account = '{0}';", account);
            List<string> uidList = new List<string>();          
            if (this.OpenConnection())
            {
                //如果数据库打开成功
                MySqlCommand sqlCommand = new MySqlCommand(query, SqlConnection);
                MySqlDataReader mySqlDataReader = sqlCommand.ExecuteReader();
                //sqlCommand.ExecuteNonQuery();

                while (mySqlDataReader.Read())
                {
                    uidList.Add(mySqlDataReader["account"] + "");                   
                }

                mySqlDataReader.Close();
                CloseConnection();

                if(uidList.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public AccountModle GetAccountModleByAcc(string account)
        {
            if (IsAccontExist(account) && OpenConnection())
            {
                string query = string.Format("select * from account where account = '{0}';", account);
                AccountModle accountModle = null;

                MySqlCommand sqlCommand = new MySqlCommand(query, SqlConnection);
                MySqlDataReader mySqlDataReader = sqlCommand.ExecuteReader();


                while (mySqlDataReader.Read())
                {
                    int id = AccountCache.Instance.ID.Add_Get();
                    string pwd = mySqlDataReader["password"] + "";
                    accountModle = new AccountModle(id, account, pwd);
                }

                mySqlDataReader.Close();
                CloseConnection();

                return accountModle;
            }
            return null;
            //throw new Exception("该帐号的数据在数据库中不存在GetAccountModleByAcc()");
        }


        /// <summary>
        /// 增添用户信息
        /// </summary>
        /// <param name="userModel"></param>
        public void AddUser(UserModel userModel)
        {
            if (OpenConnection())
            {
                try
                {
                    string query = string.Format("insert into user values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', {7});",
                    userModel.Name, userModel.Money, userModel.WinCount, userModel.LoseCount, userModel.RunCount, userModel.Lv, userModel.Exp, userModel.Account);
                    MySqlCommand mySqlCommand = new MySqlCommand(query, SqlConnection);

                    mySqlCommand.ExecuteNonQuery();

                    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
                finally
                {
                    CloseConnection();
                }
            }
        }

        public bool IsUserExist(string account)
        {
            if (OpenConnection())
            {
                string query = string.Format("select * from user where account = '{0}';", account);
                MySqlCommand mySqlCommand = new MySqlCommand(query, SqlConnection);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();


                while (mySqlDataReader.Read())
                {
                    mySqlDataReader.Close();
                    CloseConnection();
                    return true;
                }

                mySqlDataReader.Close();
                CloseConnection();
            }
            return false;
        }

        public UserModel GetUserModleByAcc(string account)
        {
            if (IsUserExist(account) && OpenConnection())
            {
                string query = string.Format("select * from user where account = '{0}';", account);
                MySqlCommand mySqlCommand = new MySqlCommand(query, SqlConnection);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

                while (mySqlDataReader.Read())
                {
                    int id = UserCache.Instance.ID.Add_Get();
                    string name = (string)mySqlDataReader["Name"];
                    int money = (int)mySqlDataReader["Money"];
                    int wincount = (int)mySqlDataReader["WinCount"];
                    int losecount = (int)mySqlDataReader["LoseCount"];
                    int runcount = (int)mySqlDataReader["RunCount"];
                    int lv = (int)mySqlDataReader["Level"];
                    int exp = (int)mySqlDataReader["Exp"];

                    int accountid = AccountCache.Instance.GetOnlineID(account);

                    UserModel userModel = new UserModel();
                    userModel.Change(id, name, money, wincount, losecount, runcount, lv, exp, accountid, account);


                    mySqlDataReader.Close();
                    CloseConnection();

                    return userModel;
                }

                mySqlDataReader.Close();
                CloseConnection();
            }
            return null;
        }

    }
}
