namespace Serv4Fish3.Tools.ObjectPool
{
    public class GeneratorMc
    {
        IDistributorMc _dis;

        public GeneratorMc(IDistributorMc val)
        {
            this.init(val);
        }

        void init(IDistributorMc val)
        {
            this._dis = val;
        }

        public IObjectMc getObj(ObjectMcType type)
        {
            IObjectMc vo = this._dis.getVO(type);
            if (vo == null)
            {
                vo = this.createVO(type);
                this._dis.addVO(vo);
                vo.reset();
            }
            return vo;
        }

        private IObjectMc createVO(ObjectMcType type)
        {
            switch (type)
            {
                case ObjectMcType.FISH:
                    return new FishData();
                    //            case ObjType.COIN:
                    //                return new Coin();
            }
            return null;
        }
    }
}
