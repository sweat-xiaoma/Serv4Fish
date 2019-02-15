using System;
using System.Collections.Generic;

namespace Serv4Fish3.Tools.ObjectPool
{
    public class DistributorMc : IDistributorMc
    {
        // 使用中的对象
        Dictionary<int, IObjectMc> _UsedPool = new Dictionary<int, IObjectMc>();
        // 未使用的对象
        Dictionary<int, IObjectMc> _IdlePool = new Dictionary<int, IObjectMc>();

        public void distribution(IObjectMc val)
        {
            if (val.IsIdle)
            {
                this._IdlePool[val.GetHashCode()] = val;
                this._UsedPool.Remove(val.GetHashCode());
            }
            else
            {
                this._UsedPool[val.GetHashCode()] = val;
                this._IdlePool.Remove(val.GetHashCode());
            }
        }

        public void addVO(IObjectMc val)
        {
            val.setProtocol(this);
            if (val.IsIdle)
            {
                this._IdlePool[val.GetHashCode()] = val;
            }
            else
            {
                //this._UsedPool[val.hashc] = val;
                this._UsedPool[val.GetHashCode()] = val;
            }
        }

        public IObjectMc getVO(ObjectMcType type)
        {
            foreach (IObjectMc item in this._IdlePool.Values)
            {
                if (item.Type == type)
                {
                    item.reset();
                    return item;
                }
            }
            return null;

        }

        public void clear()
        {
            //foreach (IObjectMc item in this._IdlePool.Values)
            //{
            //    item.del();
            //}

            this._IdlePool = null;
            this._IdlePool = new Dictionary<int, IObjectMc>();
        }

        // 测试输出 
        public void look()
        {
            Console.WriteLine("[LOOK]");
            Console.WriteLine("----------- IdlePool 空闲池子 -----------");
            int num = 0;
            foreach (int key in this._IdlePool.Keys)
            {
                num++;
                Console.WriteLine("KEY:" + key + ",type:" + this._IdlePool[key].Type);
            }

            Console.WriteLine("-----------共" + num + "个空闲对象-----------");
            num = 0;
            Console.WriteLine("----------- _UsedPool 繁忙池子 -----------");
            foreach (int key in this._UsedPool.Keys)
            {
                num++;
                Console.WriteLine("KEY:" + key + ",type:" + this._UsedPool[key].Type);
            }
            Console.WriteLine("-----------共" + num + "个繁忙对象-----------");
        }
    }
}