using System;
using MySql.Data.MySqlClient;


namespace Serv4Fish3.Model
{
    public class Fish
    {
        /** 静态数据 start **/
        public readonly int ID;
        public readonly int Life; // 生命值
        public readonly int Speed; // 游动速度
        public readonly int Kill_bonus; // 奖励
        public readonly int Count_max; // 最多数量
        /** 静态数据 end **/

        public Fish(int id, int life, int speed, int kill_bonus, int count_max)
        {
            this.ID = id;
            this.Life = life;
            this.Speed = speed;
            this.Kill_bonus = kill_bonus;
            this.Count_max = count_max;


            //if ()
            //fishvo.Count_max / 2 + 1

            if (this.Count_max / 2 + 1 > this.Count_max)
            {
                Console.WriteLine("[" + DateTime.Now + "] 读取静态数据配置错误, 数量错误: " + id);
            }

            if (this.Speed <= 0 || this.Life <= 0 || this.Kill_bonus <= 0 || this.Count_max <= 0)
            {
                Console.WriteLine("[" + DateTime.Now + "] 读取静态数据配置错误, 设置不完整: " + id);
            }

            if (this.Speed / 2 > this.Speed)
            {
                Console.WriteLine("[" + DateTime.Now + "] 读取静态数据配置错误, 速度错误: " + id);
            }


        }
    }
}
