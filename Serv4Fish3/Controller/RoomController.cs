using System;
using System.Text;
using FishCommon3;
using Serv4Fish3.ServerSide;

namespace Serv4Fish3.Controller
{
    public class RoomController : BaseController
    {
        public RoomController()
        {
            requestCode = RequestCode.Room;

        }


        public string JoinRoom(string data, Client client, Server server)
        {
            // 快速游戏
            Room room = server.JoinRoomFast(client);
            string roomData = room.GetRoomData();
            // 广播给其他客户端有人来了
            room.BroadcastMessage(client, ActionCode.UpdateRoom, roomData);
            return ((int)ReturnCode.Success).ToString() + "-" + roomData;
        }

        //public string QuitRoom(string data, Client client, Server server)
        //{
        //    bool isRoomOwner = client.IsRoomOwner();

        //    Room room = client.Room;
        //    if (isRoomOwner) // 房主退出 关闭房间或转移房主
        //    {
        //        // 告诉房主关闭房间
        //        room.BroadcastMessage(client, ActionCode.QuitRoom, ((int)ReturnCode.Success).ToString());
        //        // 销毁房间
        //        room.Close();
        //        return ((int)ReturnCode.Success).ToString();
        //    }
        //    else
        //    {
        //        room.RemoveClient(client);

        //        // 广播有人退出给房间内的其他客户端
        //        room.BroadcastMessage(client, ActionCode.UpdateRoom, room.GetRoomData());

        //        return ((int)ReturnCode.Success).ToString(); // 当前客户端退出 回到房间列表
        //    }
        //}

    }
}
