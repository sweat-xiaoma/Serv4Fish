namespace Serv4Fish3.Tools.ObjectPool
{
    public interface IObjectMc
    {
        //int HashCode { get; set; }

        // 类型标识
        ObjectMcType Type { get; set; }

        // 标记是否空闲
        bool IsIdle { get; set; }

        // 释放对象的内部引用
        void dispose();

        // 彻底释放对象
        void del();

        // 重置
        void reset();

        // 设置协议
        void setProtocol(IDistributorMc val);

        // 动作
        void action();
    }
}
