using System;
using FishCommon3;
using Serv4Fish3.ServerSide;
using Serv4Fish3.DAO;
using Serv4Fish3.Model;

namespace Serv4Fish3.Controller
{
    public class UserController : BaseController
    {
        UserDAO userDAO = new UserDAO();
        ResultDAO resultDAO = new ResultDAO();
        public UserController()
        {
            requestCode = RequestCode.User;
        }

        public string Login(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            User user = userDAO.VerifyUser(client.MySQlConn, strs[0], strs[1]);
            if (user == null)
            {
                //Enum.GetName(typeof(ReturnCode), ReturnCode.Fail);
                //return ((int)ReturnCode.Fail).ToString();
                //return Enum.GetName(typeof(ReturnCode), ReturnCode.Fail);
                return ((int)ReturnCode.Fail).ToString();
            }
            else
            {
                Result result = resultDAO.GetResultByUserid(client.MySQlConn, user.id);
                client.SetUserData(user, result);
                return string.Format("{0},{1},{2},{3}",
                                     ((int)ReturnCode.Success).ToString(),
                                     user.username,
                                     result.Totalresult,
                                     result.Winresult);
            }
        }
    }
}
