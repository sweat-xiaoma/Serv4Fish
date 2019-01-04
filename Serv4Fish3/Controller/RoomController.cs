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

        //public string CreateRoom(string data, Client client, Server server)
        //{
        //    server.CreateRoom(client);

        //    // 创建条件后续添加
        //    //return ((int)ReturnCode.Success).ToString();
        //    return ((int)ReturnCode.Success).ToString() + "," + ((int)RoleType.Blue).ToString();
        //}

        //public string ListRoom(string data, Client client, Server server)
        //{
        //    StringBuilder sb = new StringBuilder(); // 房间列表
        //    foreach (Room room in server.ListRoom())
        //    {
        //        if (room.IsWaitingJoin())
        //        {
        //            sb.Append(room.GetHouseOwnerData() + "|"); // 等待中的房间列表 用竖线分隔
        //        }
        //    }

        //    if (sb.Length == 0)
        //    {
        //        sb.Append("0"); // 0 代表没有空房间
        //    }
        //    else
        //    {
        //        sb.Remove(sb.Length - 1, 1); // 移除最后一个 竖线
        //    }
        //    return sb.ToString();

        //}

        public string JoinRoom(string data, Client client, Server server)
        {
            //int roomId = int.Parse(data);
            //Room room = server.GetRoomById(roomId);
            //if (room == null)
            //{
            //    // 没有找到房间
            //    return ((int)ReturnCode.Notdfound).ToString();
            //}
            //else if (room.IsWaitingJoin() == false)
            //{
            //    // 不在等待开局状态不能加入 已经满员了
            //    return ((int)ReturnCode.Fail).ToString();
            //}
            //else
            //{
            //    room.AddClient(client);

            //    string roomData = room.GetRoomData();
            //    // 广播的内容不用加 returncode ，一定成功的
            //    room.BroadcastMessage(client, ActionCode.UpdateRoom, roomData);
            //    // returncode-roletype-id1,username1,totalresult1,winresult1|id2,username2,totalresult2,winresult2
            //    return ((int)ReturnCode.Success).ToString() + "-" + ((int)RoleType.Blue).ToString() + "-" + roomData;
            //}



            // 快速游戏
            Room room = server.JoinRoomFast(client);
            string roomData = room.GetRoomData();
            // 广播给其他客户端有人来了
            room.BroadcastMessage(client, ActionCode.UpdateRoom, roomData);
            return ((int)ReturnCode.Success).ToString() + "-" + roomData;


            //return 


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
