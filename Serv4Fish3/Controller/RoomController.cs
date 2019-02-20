using System;
using System.Text;
using FishCommon3;
using Serv4Fish3.ServerSide;
using Serv4Fish3.Tools;

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
            //Console.WriteLine("JoinRoom hashcode: " + client.GetHashCode());
            // 快速游戏
            Room room = server.JoinRoomFast(client, client.GetUser().EnterScene);
            string data22 = room.GetRoomData(); // 返回guid集合11111查询guid,对应的鱼的位置

            // 广播给其他客户端有人来了
            room.BroadcastMessage(client, ActionCode.UpdateRoom, data22);

            //// 每个新玩家接收一次同步即可.
            //room.BroadcastMessage(client, ActionCode.FishGenerateJoinRoomA, client.GetUser().Corner + "");

            return ((int)ReturnCode.Success).ToString() + "," + (int)room.SceneIndex + "-" + data22;
        }

        public string FishGenerateJoinRoomA(string data, Client client, Server server)
        {
            //return "";


            //Console.WriteLine("--------- 33: " + data);
            //string[] strs = data.Split('-');
            string[] strs = data.Split('(');
            int corner = int.Parse(strs[0]);
            Client clientNew = client.Room.GetClient(corner);
            if (!string.IsNullOrEmpty(strs[1]))
                clientNew.Send(ActionCode.FishGenerateJoinRoomB, strs[1]);

            return "";
        }

        //public string FishGenerateJoinRoom(string data, Client client, Server server)
        //{


        //}

        //public string StepOutRoom(string data, Client client, Server server)
        //{
        //    //Room room = server.

        //}

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

        public string GameSkill(string data, Client client, Server server)
        {
            GameSkillCode skillIndex = (GameSkillCode)int.Parse(data);

            Room room = client.Room;
            if (skillIndex == FishCommon3.GameSkillCode.FROZEN)
            {
                bool re = room.StartFrozen();
                if (re)
                {
                    string data71 = (int)ReturnCode.Success + "|"
                         //+ 1 + "|" + client.GetUser().Corner + "|" + Defines.SKILL_ICE_DURATION;
                         + client.GetUser().Corner + "|"
                         + (int)GameSkillCode.FROZEN + "|"
                         + Defines.SKILL_ICE_CD;
                    room.BroadcastMessage(null, ActionCode.GameSkill, data71);
                }
                else // 冰冻中 不能再冰冻
                {
                    string data79 = ((int)ReturnCode.Fail).ToString() + "|" + client.GetUser().Corner;
                    client.Send(ActionCode.GameSkill, data79);
                }

                // 扣钻石
                int costDiamond = 100;
                if (client.GetWallet().Diamond >= costDiamond)
                {
                    client.GetWallet().Diamond -= costDiamond;
                    string data72 = client.GetUser().Corner + "|" + client.GetWallet().Diamond;
                    room.BroadcastMessage(null, ActionCode.UpdateDiamond, data72);
                }
                else
                {
                    string data96 = ((int)ReturnCode.Fail).ToString() + client.GetUser().Corner;
                    client.Send(ActionCode.GameSkill, data96);
                }
            }
            //else if (skillIndex == 2)
            else
            {
                //costDiamond = 200;
            }


            return "";
        }
    }
}
