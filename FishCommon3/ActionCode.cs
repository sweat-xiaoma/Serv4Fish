using System;
namespace FishCommon3
{
    public enum ActionCode
    {
        None,
        Login, // 登陆
        Register, // 注册
        RoomList, // 获取房间列表
        CreateRoom, // 创建房间
        JoinRoom, // 加入房间
        UpdateRoom, // 新玩家加入 更新房间
        QuitRoom, // 退出房间
        StartGame, // 开始游戏 按钮
        ShowTimer, // 开始倒计时
        StartPlay, // 正式开始游戏
        Move, // 角色移动
        Shoot, // 射击
        Rotate, // 旋转
        // FishSync, // 鱼的同步
        FishGenerate, // 鱼的生成
        FishHit, // 打到鱼
        FishDead, // 鱼被打死
        // FishBonus, // 鱼掉钱 （暂时和上一个合并）
        FishOutScreen, // 鱼游出屏幕
        ChangeCost, // 改变消耗
        UpdateMoney, // 更新金币显示
        UpdateDiamond, // 更新钻石显示
        // HeartBeatServ, // 服务端发起心跳
        StepOutRoom, // 暂时离开房间

        PingFromServ, // 服务端 -> Client
        PongFromClient,

        PingFromClient, // 客户端 -> Server // ------ 暂时没用
        PongFromServ,

        UpgradeCannon, // 升级火炮
        GameSkill, // 技能
        GameSkillFocusOnFish, // 锁定某条鱼

        SkillFrozenOver, // 技能 -- 冷冻结束

        FishGenerateJoinRoomA, // 查询鱼的数据 同步给新加入的玩家

        FishGenerateJoinRoomB, // 新玩家接收老鱼的数据

    }
}
