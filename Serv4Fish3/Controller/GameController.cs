using System;
using FishCommon3;
using Serv4Fish3.ServerSide;

namespace Serv4Fish3.Controller
{
    public class GameController : BaseController
    {
        public GameController()
        {
            requestCode = RequestCode.Game;
        }

        public string StartGame(string data, Client client, Server server)
        {
            if (client.IsRoomOwner()) // 房主才能开始
            {
                Room room = client.Room;
                room.BroadcastMessage(client, ActionCode.StartGame, ((int)ReturnCode.Success).ToString()); // 给其他客户端广播 开始游戏

                room.StartTimer(); // 开始倒计时

                return ((int)ReturnCode.Success).ToString(); // 给当前客户端发送 开始游戏
            }
            else
            {
                return ((int)ReturnCode.Fail).ToString();
            }
        }
    }
}
