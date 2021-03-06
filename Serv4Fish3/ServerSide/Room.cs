﻿#define DEBUG_VIEW
#define DEBUGVIEW0214
using System;
using System.Collections.Generic;
using System.Text;
using FishCommon3;
using Serv4Fish3.Tools;
using Serv4Fish3.Model;
using System.Collections.Concurrent;
using Serv4Fish3.Tools.ObjectPool;

namespace Serv4Fish3.ServerSide
{
    public class Room
    {
        ConcurrentDictionary<string, FishData> fishCDic = new ConcurrentDictionary<string, FishData>();
        public int iceLeft; // 剩余冰冻时间 
        public const int MaxPeople = 4; // 1;
        Client[] clientArray = new Client[MaxPeople];
        Server server;
        // 房间的场景号
        public readonly SceneIndex SceneIndex;
        int fishguidprefix = 0;

        public Room(Server server, SceneIndex sceneIndex)
        {
            this.server = server;
            this.SceneIndex = sceneIndex;

            // 初始化对象池
            this.InitDistributorAndGenerator();
            this.fishguidprefix = this.GetHashCode();
        }

        DistributorMc _objDistributor;
        GeneratorMc _objGenerator;

        void InitDistributorAndGenerator()
        {
            if (this._objDistributor == null)
            {
                this._objDistributor = new DistributorMc();
            }
            if (this._objGenerator == null)
            {
                this._objGenerator = new GeneratorMc(this._objDistributor);
            }
        }

        void DestroyDistributorAndGenerator()
        {
            this._objDistributor.clear();
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

        public Client GetClient(int corner)
        {
            foreach (Client item in this.clientArray)
            {
                if (item != null && item.GetUser().Corner == corner)
                    return item;
            }
            return null;
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

        // 销毁房间
        void DestroyRoom()
        {
            Console.WriteLine("164 DestroyRoom~~");
            server.RemoveRoom(this);

            // 清空对象池
            this.DestroyDistributorAndGenerator();
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

        ////todo test
        //int generateIndex;
        ////todo test
        //int deadIndex;

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
                    //server.SendResponse2Client(client, actionCode, data);
                    client.Send(actionCode, data);
                }
            }

#if DEBUG_VIEW

            //if (actionCode == ActionCode.FishGenerate)
            //{
            //    generateIndex++;
            //    Console.WriteLine("[" + DateTime.Now + "] " + "房间[{0}]内广播消息[{1}]给[{2}]   ->  [{3}]", this.GetHashCode(), actionCode, sb, data);
            //    Console.WriteLine("出生: " + generateIndex + "  count:" + this.fishCDic.Count);
            //}


            //if (actionCode == ActionCode.FishDead)
            //{
            //    deadIndex++;
            //    Console.WriteLine("[" + DateTime.Now + "] 死亡" + data);
            //    Console.WriteLine("死亡: " + deadIndex);
            //}


            ////todo test start
            //Console.WriteLine("[" + DateTime.Now + "] [广播] ActionCode: {0} data: {1}", actionCode, data);
            ////todo test end
#endif

        }

        public void HitFish(Client client, string fishguid)
        {
            //#if DEBUGVIEW0214
            //            Console.WriteLine("");
            //            Console.WriteLine("");
            //            Console.WriteLine("245 -------------------------");
            //            Console.WriteLine(fishguid);
            //#endif

            //todo test start
            Console.WriteLine("client Hashcode: " + client.GetHashCode());
            //todo test end
            if (fishCDic.ContainsKey(fishguid))
            {
                FishData findFish = fishCDic[fishguid];
                Console.WriteLine("包含鱼: " + fishguid + " coin:" + findFish.coin + " 元hp:" + findFish.hp);
                findFish.hp -= client.GetUser().CannonLvCurr;
#if DEBUGVIEW0214
                Console.WriteLine("lvCurr: " + client.GetUser().CannonLvCurr);
                Console.WriteLine("包含鱼: " + fishguid + " coin:" + findFish.coin + " 新hp:" + findFish.hp);
                //Console.WriteLine("");
#endif
                if (findFish.hp <= 0)
                {
#if DEBUGVIEW0214
                    //Console.WriteLine("kill by corner: " + client.GetUser().Corner
                    //+ " hashcode: " + client.GetHashCode());
                    Console.WriteLine("移除鱼: " + fishguid);
#endif

                    int killCorner = client.GetUser().Corner;
                    //Console.WriteLine("打死了鱼: " + findFish.diamond + " hashCode: " + findFish.GetHashCode()
                    //+ " HashCode: " + findFish.GetHashCode() % 2);


                    //// 随机一下发不发钻石
                    //if (findFish.diamond > 0 && findFish.GetHashCode() % 2 == 0)
                    if (findFish.diamond > 0)
                    {
                        // 发钻石
                        client.GetWallet().Diamond += findFish.diamond;
                        string data301 = killCorner + "|" + client.GetWallet().Diamond;
                        this.BroadcastMessage(null, ActionCode.UpdateDiamond, data301);
                    }
                    if (findFish.coin > 0)
                    {
                        // 发金币
                        // 玩家 - 加钱 (广播 同步金币)
                        client.GetWallet().Money += findFish.coin;
                        string data294 = killCorner + "|" + client.GetWallet().Money;
                        this.BroadcastMessage(null, ActionCode.UpdateMoney, data294);
                    }
                    string dataFishDead = killCorner + "|" + fishguid + "|" + findFish.diamond + "|" + findFish.coin;
                    this.BroadcastMessage(null, ActionCode.FishDead, dataFishDead);

                    //todo test  
                    //lock (fishDic)
                    //fishDic.Remove(fishguid); // 移除掉
                    //fishDic.TryRemove(fishguid);
                    if (!fishCDic.TryRemove(fishguid, out FishData fishData))
                    {
                        Console.WriteLine("杀死鱼 tryRemove1 失败" + fishguid);
                    }
                    fishData.dispose(); // todo new

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
                Console.WriteLine(" !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! 没找到鱼: " + fishguid + " ~ " + fishCDic.Count);
#endif
            }
        }

        public void FishOutByClient(Client client, string fishguid)
        {
            if (fishCDic.ContainsKey(fishguid))
            {
                FishData findData = fishCDic[fishguid];
                if (!fishCDic.TryRemove(fishguid, out FishData fishData))
                {
                    Console.WriteLine("出屏 tryRemove2 失败" + fishguid);
                }
                fishData.dispose();
            }
        }
        // 生成鱼
        public void GenerateFishs(Fish fishvo)
        {
            if (this.iceLeft > 0) // 冰冻中
            {
                this.iceLeft--;
                if (this.iceLeft <= 0)
                {
                    Console.WriteLine("解冻~");
                    // 广播解冻
                    this.BroadcastMessage(null, ActionCode.SkillFrozenOver, "aa");
                }
                return;
            }

            fishGenLoop++;
            if (fishGenLoop > 100) // 100*.5 = 50秒
            {
                CheckOldFish();
            }

            Random random = new Random();
            if (this.fishCDic.Count < 50)
            {
                //生成鱼
                int moveType = random.Next(0, 2); // 0 直走; 1转弯
                                                  // todo test
                                                  //moveType = 1; // 转弯
                moveType = 0; // 直走
                int angOffset = 0, angSpeed = 0;

                int genPosIndex = random.Next(0, 16); // 出生位置
                if (moveType == 0)
                {
                    switch (genPosIndex)
                    {
                        case 0:
                        case 2:
                            angOffset = random.Next(10, 30);
                            break;
                        case 1:
                        case 3:
                            angOffset = random.Next(-30, -10);
                            break;
                        case 4:
                        case 9:
                            angOffset = random.Next(-20, -5);
                            break;
                        case 5:
                        case 8:
                            angOffset = random.Next(-10, 10);
                            break;
                        case 6:
                        case 7:
                            angOffset = random.Next(-5, 20);
                            break;
                        case 10:
                        case 15:
                            angOffset = random.Next(-20, 50);
                            break;
                        case 11:
                        case 14:
                            angOffset = random.Next(-40, 30);
                            break;
                        case 12:
                        case 13:
                            angOffset = random.Next(-50, 20);
                            break;
                    }
                    GenFish111(fishvo, genPosIndex, angOffset, 0);
                }
                else
                {
                    if (random.Next(0, 2) == 0)
                    {
                        //    angSpeed = random.Next(-15, -9);
                        angSpeed = 5;
                    }
                    else
                    {
                        angSpeed = -5;
                        //    angSpeed = random.Next(9, 15);
                    }
                    //todo test start
                    genPosIndex = 0;
                    //todo test end

                    GenFish111(fishvo, genPosIndex, 0, angSpeed);
                }
            }
        }


        /// <summary>
        /// Gens the fish111.
        /// </summary>
        /// <param name="fishvo"> 鱼的静态数据.</param>
        /// <param name="angOffset">直走倾斜角.</param>
        /// <param name="angSpeed">转弯角速度.</param>
        void GenFish111(Fish fishvo, int genPosIndex, int angOffset, int angSpeed)
        {
            Random random = new Random();

            int amount = random.Next(fishvo.Count_max / 2 + 1, fishvo.Count_max); // 这一批的鱼数量
            int speed = random.Next(fishvo.Speed / 2, fishvo.Speed);

            // todo generator
            this.fishguidprefix++;
            Console.WriteLine("");
            Console.WriteLine("");
            for (int i = 0; i < amount; i++)
            {
                FishData fishData = this._objGenerator.getObj(ObjectMcType.FISH) as FishData;
                fishData.hp = fishvo.Life;
                //todo test
                //fishData.hp = 10;
                fishData.coin = fishvo.Kill_bonus;
                fishData.birthTime = Util.GetTimeStamp;
                fishData.diamond = fishvo.Kill_bonus_Diamond;
                // 毫秒加层数
                string fishguid = this.fishguidprefix + "" + i;
                //if (fishCDic.ContainsKey(fishguid))
                //{
                //    Console.WriteLine("已经包含: " + fishguid);
                //    //fishCDic[fishguid] = fishData;
                //    // todo test
                //    fishCDic.TryUpdate(fishguid, fishData, null);
                //}
                //else
                if (!fishCDic.ContainsKey(fishguid))
                {
                    //fishDic.Add(fishguid, fishData);
                    if (!fishCDic.TryAdd(fishguid, fishData))
                    {
                        Console.WriteLine("新增鱼失败 tryRemove3 失败" + fishguid);
                    }
                    else
                    {
                        Console.WriteLine("[产生新鱼] guid:{0} index:{1} hp:{2}",
                                    fishguid, fishvo.ID, fishvo.Life);
                    }
                }
            }

            string data = this.fishguidprefix + "|"  // 0
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
        }

        void joinRoomFishes()
        {
            string guid = "";
            int kill_bonus = 1;
            int speed = 2;
            float angle = 0f;
            int angSpeed = 4;
            int angOffset = 3;
            string data = guid + "|"
                + kill_bonus + "|"
                + speed + "|"
                + angle + "|"
                + angSpeed + "|"
                + angOffset + "|"
                ;
        }

        public bool StartFrozen()
        {
            if (this.iceLeft <= 0)
            {
                this.iceLeft = Defines.SKILL_ICE_DURATION;
                return true;
            }
            return false;
        }

        public int fishGenLoop;

        void CheckOldFish()
        {
            //Console.WriteLine("清理老鱼!");
            fishGenLoop = 0;
            long old = Util.GetTimeStamp - 100;
            //long old = Util.GetTimeStamp - 10;

            List<string> oldFishs = new List<string>();
            // 清理一次 防止意外没清理掉的死鱼.
            foreach (KeyValuePair<string, FishData> item in this.fishCDic)
            {
                if (item.Value.birthTime < old)
                {
                    oldFishs.Add(item.Key);
                }
            }

            Console.WriteLine("清理老鱼: " + oldFishs.Count);

            for (int i = 0; i < oldFishs.Count; i++)
            {
                if (!fishCDic.TryRemove(oldFishs[i], out FishData fishData))
                {
                    Console.WriteLine("很老的鱼了 tryRemove3 失败" + oldFishs[i]);
                }
                fishData.dispose(); // todo new
            }
        }


    }
}
