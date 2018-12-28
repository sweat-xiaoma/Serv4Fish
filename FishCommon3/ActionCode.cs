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

    }
}
