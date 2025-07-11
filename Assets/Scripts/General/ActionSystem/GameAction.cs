using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 注册游戏事件
/// </summary>
public abstract class GameAction 
{
    //存储游戏动作前发生的反应
    public List<GameAction> PreReactions {get; private set;} = new List<GameAction>();
    //动作中发生
    public List<GameAction> PerformReactions {get; private set;} = new List<GameAction>();
    //动作后发生
    public List<GameAction> PostReactions {get; private set;} = new List<GameAction>();
}
