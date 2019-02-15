#define DEBUG_VIEW
#define DEBUGVIEW0214
using System;
using System.Collections.Generic;
using System.Text;
using FishCommon3;
using Serv4Fish3.Tools;
using Serv4Fish3.Model;
using System.Collections.Concurrent;


namespace Serv4Fish3.ServerSide
{
    //enum RoomState
    //{
    //    WaitingJoin, // 等待人齐
    //    WaitingBattle, // 等待开战
    //    Battle, // 
    //    End
    //}

    public class FishData
    {
        public int hp; // 血量
        public int coin; // 金币
    }


    public class Room
    {
        //Dictionary<long, FishData> fishDic = new Dictionary<long, FishData>();
        ConcurrentDictionary<long, FishData> fishDic = new ConcurrentDictionary<long, FishData>();


        public const int MaxPeople = 4; // 1;

        Client[] clientArray = new Client[MaxPeople];
        //RoomState state = RoomState.WaitingJoin;

        Server server;

        public Room(Server server)
        {
            this.server = server;
        }

        //// 房间等待加入
        //public bool IsWaitingJoin()
        //{
        //    return state == RoomState.WaitingJoin;
        //}

        public void AddClient(Client client, int index)
        {
            // 判断是否有房主 没有房主的话 自己当房主
            // 正常情况除非房内没人，不然是有房主的
            bool hasMaster = false;
            foreach (Client item in clientArray)
            {
                if (item != null && item.isMaster == 1) // 房主
                {
                    hasMaster = true;
                    break;
                }
            }

            if (hasMaster == false)
            {
                client.isMaster = 1;
            }

            //clientList.Add(client);
            if (clientArray[index] == null)
            {
                clientArray[index] = client;
                client.Room = this;
                client.GetUser().Corner = index;
            }
            else
            {
                Console.WriteLine("[" + DateTime.Now + "] " + "[Room AddClient] 用户进房间座位有人了");
            }

            //if (clientList.Count >= Room.MaxPeople) // 最大人数
            //{
            //    state = RoomState.WaitingBattle; // 满员了 等待开始战斗
            //}
        }

        //public void RemoveClient(Client client)
        public void RemoveClient(Client client, int index)
        {
            client.Room = null;
            //clientList.Remove(client);
            clientArray[index] = null;
        }

        public void QuitRoom(Client client)
        {
            // 退出的人 是否是房主
            bool quitIsMaster = client.isMaster == 1;

            // 从座位上移除
            this.RemoveClient(client, client.GetUser().Corner);

            // 判断房间里没人了就销毁房间
            bool isEmpty = true;
            foreach (var item in clientArray)
            {
                if (item != null)
                {
                    isEmpty = false;
                    break;
                }
            }
            if (isEmpty)
            {
                DestroyRoom();
            }
            else
            {
                if (quitIsMaster) // 退出的是房主， 且房间里还有人 -》 转移房主
                {
                    foreach (Client item in clientArray)
                    {
                        if (item != null) // 剩余人的第一个当房主
                        {
                            item.isMaster = 1;
                            break;
                        }
                    }
                }
            }

            // 广播给房间内其他人 我走了～再见。
            BroadcastMessage(client, ActionCode.UpdateRoom, GetRoomData());
        }

        //public void StepOut(Client client)
        //{
        //    //if (client.)
        //    foreach (Client item in this.clientArray)
        //    {
        //        //if (item == client && client.isMaster == 1)
        //        if (item == client)
        //        {
        //            if (client.isMaster != 1)
        //            {
        //                // 暂离的用户不是房主 不用刷新房间了
        //                return;
        //            }
        //            else
        //            {
        //                client.isMaster = 0; // 先把房主撤掉
        //                // 选第一个人继续当房主
        //                foreach (Client item2Master in this.clientArray)
        //                {
        //                    //if (client.)
        //                }
        //            }
        //            break;
        //        }
        //    }
        //}

        // 销毁房间
        void DestroyRoom()
        {
            server.RemoveRoom(this);
        }

        /// <summary>
        /// 获取一个空座位
        /// </summary>
        /// <returns>获得座位号，-1表示没有空位.</returns>
        public int EmptySeat()
        {
            for (int i = 0; i < clientArray.Length; i++)
            {
                if (clientArray[i] == null)
                {
                    return i;
                }
            }
            return -1;
        }

        public string GetRoomData() // 房间里的用户信息
        {
            StringBuilder sb = new StringBuilder();
            foreach (Client client in clientArray)
            {
                if (client == null)
                    sb.Append("|");
                else
                    sb.Append(client.GetUserData() + "|");
            }

            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }

        //todo test
        int generateIndex;
        //todo test
        int deadIndex;

        /// <summary>
        /// 房内广播
        /// </summary>
        /// <param name="excludeClient">不接受广播的用户 一般是当前客户端不用再接收</param>
        public void BroadcastMessage(Client excludeClient, ActionCode actionCode, string data)
        {
#if DEBUG_VIEW
            StringBuilder sb = new StringBuilder();
#endif
            foreach (Client client in clientArray)
            {
                if (client != null && client != excludeClient)
                {
#if DEBUG_VIEW
                    sb.Append(client.GetUser().Username + ",");
#endif 
                    server.SendResponse(client, actionCode, data);
                }
            }

#if DEBUG_VIEW

            if (actionCode == ActionCode.FishGenerate)
            {
                generateIndex++;
                Console.WriteLine("[" + DateTime.Now + "] " + "房间[{0}]内广播消息[{1}]给[{2}]   ->  [{3}]", this.GetHashCode(), actionCode, sb, data);
                Console.WriteLine("出生: " + generateIndex);
            }


            if (actionCode == ActionCode.FishDead)
            {
                deadIndex++;
                Console.WriteLine("[" + DateTime.Now + "] 死亡" + data);
                Console.WriteLine("死亡: " + deadIndex);
            }
#endif

        }

        public void HitFish(Client client, long fishguid)
        {
#if DEBUGVIEW0214
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("245 -------------------------");
            Console.WriteLine(fishguid);
#endif
            if (fishDic.ContainsKey(fishguid))
            {

                FishData findFish = fishDic[fishguid];

                findFish.hp -= client.GetUser().CannonLvCurr;

#if DEBUGVIEW0214
                Console.WriteLine("包含鱼: " + fishguid + " hp:" + findFish.hp);
#endif

                if (findFish.hp <= 0)
                {
#if DEBUGVIEW0214
                    Console.WriteLine("移除鱼: " + fishguid);
#endif

                    int killCorner = client.GetUser().Corner;
                    // 广播 鱼死了,发钱
                    string data2 = killCorner + "|" + fishguid + "|" + findFish.coin;
                    this.BroadcastMessage(null, ActionCode.FishDead, data2);

                    // 玩家 - 加钱 (广播 同步金币)
                    client.GetWallet().Money += findFish.coin;
                    string data316 = client.GetUser().Corner + "|" + client.GetWallet().Money;
                    this.BroadcastMessage(null, ActionCode.UpdateMoney, data316);

                    //todo test  
                    //lock (fishDic)
                    //fishDic.Remove(fishguid); // 移除掉
                    //fishDic.TryRemove(fishguid);
                    FishData fishData;
                    if (!fishDic.TryRemove(fishguid, out fishData))
                    {
                        Console.WriteLine("杀死鱼 tryRemove1 失败" + fishguid);
                    }

                    //#if DEBUG_VIEW
                    //                    Console.WriteLine("鱼死 鱼减少， 鱼数量: " + fishDic.Count);
                    //#endif
                }
                //#if DEBUG_VIEW
                //                else
                //                {
                //                    //Console.WriteLine("鱼没死 " + "  : " + fishguid);
                //                    Console.WriteLine("鱼没死 fishguid:{0} hp:{1} damage:{2}",
                //                    fishguid, findFish.hp, damage);
                //                }
                //#endif
            }
            else
            {
#if DEBUGVIEW0214
                Console.WriteLine(" !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! 没找到鱼: " + fishguid + " !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
#endif
            }
        }

        public void FishOutByClient(Client client, long fishguid)
        {
            if (fishDic.ContainsKey(fishguid))
            {
                FishData findData = fishDic[fishguid];
                ////todo test  
                //lock (fishDic)
                //fishDic.Remove(fishguid); // 出屏了。 移除掉。 
                //#if DEBUG_VIEW
                //                Console.WriteLine("出屏 鱼减少， 鱼数量: " + fishDic.Count);
                //#endif
                FishData fishData;
                if (!fishDic.TryRemove(fishguid, out fishData))
                {
                    Console.WriteLine("出屏 tryRemove2 失败" + fishguid);
                }
            }
        }

        //float fishGenWaitTime = 0.5f; // 鱼和鱼的生成间隔

        public int fishGenLoop;

        void CheckOldFish()
        {
            //Console.WriteLine("清理老鱼!");
            fishGenLoop = 0;
            long now = Util.GetTimeStamp() - 100;

            List<long> oldFishs = new List<long>();
            // 清理一次 防止意外没清理掉的死鱼.
            foreach (long key in this.fishDic.Keys)
            {
                if (key < now) // 很老的鱼了
                {
                    oldFishs.Add(key);


                }
            }

            for (int i = 0; i < oldFishs.Count; i++)
            {
                ////todo test线程问题
                //lock (this.fishDic)
                //this.fishDic.Remove(oldFishs[i]);

                FishData fishData;
                if (!fishDic.TryRemove(oldFishs[i], out fishData))
                {
                    Console.WriteLine("很老的鱼了 tryRemove3 失败" + oldFishs[i]);
                }
            }
        }

        // 生成鱼
        public void GenerateFishs(Fish fishvo)
        {
            //Console.WriteLine("fishGenLoop: " + this.fishGenLoop + " Time: " + DateTime.Now + ":" + DateTime.Now.Millisecond);

            fishGenLoop++;
            if (fishGenLoop > 100) // 100*.5 = 50秒
            //if (fishGenLoop > 25) // 50*.5 = 25秒 .
            {
                CheckOldFish();
            }

            Random random = new Random();
            if (this.fishDic.Count < 50)
            {
                //生成鱼
                int moveType = random.Next(0, 2); // 0 直走; 1转弯
                int angOffset, angSpeed;

                if (moveType == 0)
                {
                    angOffset = random.Next(-22, 22);
                    GenFish111(fishvo, angOffset, 0);
                }
                else
                {
                    if (random.Next(0, 2) == 0)
                    {
                        angSpeed = random.Next(-15, -9);
                    }
                    else
                    {
                        angSpeed = random.Next(9, 15);
                    }
                    GenFish111(fishvo, 0, angSpeed);
                }
            }

        }

        /// <summary>
        /// Gens the fish111.
        /// </summary>
        /// <param name="fishvo"> 鱼的静态数据.</param>
        /// <param name="angOffset">直走倾斜角.</param>
        /// <param name="angSpeed">转弯角速度.</param>
        void GenFish111(Fish fishvo, int angOffset, int angSpeed)
        {
            Random random = new Random();

            int genPosIndex = random.Next(0, 16); // 出生位置
            int amount = random.Next(fishvo.Count_max / 2 + 1, fishvo.Count_max); // 这一批的鱼数量
            int speed = random.Next(fishvo.Speed / 2, fishvo.Speed);

            FishData fishData = new FishData();
            //fishData.hp = fishvo.Life;
            //todo test
            fishData.hp = 10;
            fishData.coin = fishvo.Kill_bonus;

            //string millisecond = DateTime.Now + "" + DateTime.Now.Millisecond;
            //string millisecond = Guid.NewGuid().ToString();
            long millisecond = Util.GetTimeStamp();

            Console.WriteLine("");
            Console.WriteLine("");
            for (int i = 0; i < amount; i++)
            {
                // 毫秒加层数
                //string fishguid = (millisecond + i).ToString();
                long fishguid = millisecond + i;
                Console.WriteLine(fishguid + " ---- " + i + "/" + amount);
                if (fishDic.ContainsKey(fishguid))
                {
                    Console.WriteLine("已经包含: " + fishguid);
                    fishDic[fishguid] = fishData;
                }
                else
                {
                    //fishDic.Add(fishguid, fishData);
                    if (!fishDic.TryAdd(fishguid, fishData))
                    {
                        Console.WriteLine("新增鱼失败 tryRemove3 失败" + fishguid);
                    }
                }

            }

            string data = millisecond + "|"  // 0
                + fishvo.Life + "|"  // 1
                + fishvo.Kill_bonus + "|"  // 2
                + amount + "|"  // 3
                + genPosIndex + "|"  // 4
                + speed + "|"  // 5
                + angOffset + "|"  // 6
                + angSpeed + "|"  // 7
                + fishvo.ID + "|"  // 8
                ;

            this.BroadcastMessage(null, ActionCode.FishGenerate, data);


            ////Console.WriteLine("鱼数量：" + fishDic.Count);
            //Console.WriteLine("BroadFISH:" + this.fishDic.Count
            //        + " Time:" + System.DateTime.Now + System.DateTime.Now.Millisecond);
            //foreach (long fishguid in this.fishDic.Keys)
            //{
            //    Console.WriteLine("fishguid: " + fishguid);
            //}

            //Console.WriteLine("-----------------------");


        }



    }

}
