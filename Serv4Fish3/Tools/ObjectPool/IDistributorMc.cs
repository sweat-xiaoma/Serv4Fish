namespace Serv4Fish3.Tools.ObjectPool
{
    /// <summary>
    /// 分配器接口
    /// </summary>
    public interface IDistributorMc
    {
        // 分配
        void distribution(IObjectMc val);

        // 添加元素
        void addVO(IObjectMc val);

        // 获取元素
        //IObjectMc getVO(int type);
        IObjectMc getVO(ObjectMcType type);

        // 清除所有未使用的元素
        void clear();
    }
}
