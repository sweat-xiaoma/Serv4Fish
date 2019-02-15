using System;
namespace Serv4Fish3.Tools.ObjectPool
{
    public class FishData : IObjectMc
    {
        public int hp; // 血量
        public int coin; // 金币

        //public int HashCode
        //{
        //    get => throw new NotImplementedException();
        //    set => throw new NotImplementedException();
        //}


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
            this.IsIdle = true;
            this._dis.distribution(this);
        }

        public void reset()
        {
            this.IsIdle = false;
            this._dis.distribution(this);
        }

        public void setProtocol(IDistributorMc val)
        {
            this._dis = val;
        }
    }
}
