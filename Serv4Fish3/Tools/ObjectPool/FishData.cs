using System;
namespace Serv4Fish3.Tools.ObjectPool
{
    public class FishData : IObjectMc
    {
        public int hp; // 血量
        public int coin; // 金币
        public int diamond; // 钻石
        public long birthTime; // 出生时间

        IDistributorMc _dis;


        public ObjectMcType Type { get; set; } = ObjectMcType.FISH;

        public bool IsIdle { get; set; } = true;

        public void action() { }

        public void del()
        {
            this.dispose();
            this._dis = null;
        }

        public void dispose()
        {
            //Console.WriteLine("dispose 鱼: " + this.GetHashCode());

            this.IsIdle = true;
            this._dis.distribution(this);
        }

        public void reset()
        {
            //Console.WriteLine("reset 鱼: " + this.GetHashCode());

            this.IsIdle = false;
            this._dis.distribution(this);
        }

        public void setProtocol(IDistributorMc val)
        {
            this._dis = val;
        }
    }
}
