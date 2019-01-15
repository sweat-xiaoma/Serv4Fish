using FishCommon3;
using Serv4Fish3.ServerSide;
using Serv4Fish3.DAO;
using Serv4Fish3.Model;

namespace Serv4Fish3.Controller
{
    public class UserController : BaseController
    {
        public UserController()
        {
            requestCode = RequestCode.User;
        }

        public string Login(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            string username = strs[0];

            if (!server.CheckUserRepeat(username)) // 检测用户重复登陆
            {
                return ((int)ReturnCode.Fail).ToString();
            }

            UserDAO userDAO = new UserDAO();
            User user = userDAO.VerifyUser(client.MySQlConn, username);
            if (user == null)
            {
                return ((int)ReturnCode.Notdfound).ToString();
            }
            else
            {
                WalletDAO walletDAO = new WalletDAO();
                Wallet wallet = walletDAO.GetWalletByUsername(client.MySQlConn, user.Username);
                client.SetUserData(user, wallet); // 设置玩家信息
                return ((int)ReturnCode.Success).ToString() + ","
                                                + wallet.Money + ","
                                                + wallet.Diamond;


            }
        }

        public string ChangeCost(string data, Client client, Server server)
        {
            if (data == "a")
            {
                client.GetUser().Cost += 5; // 加倍
            }
            else
            {
                client.GetUser().Cost -= 5; // 减倍数
            }

            if (client.GetUser().Cost < 0)
            {
                client.GetUser().Cost = 0;
            }

            string re = client.GetUser().Corner + "|" + client.GetUser().Cost;

            Room room = client.Room;
            if (room != null)
                room.BroadcastMessage(null, ActionCode.ChangeCost, re);

            return "";
        }
    }
}
