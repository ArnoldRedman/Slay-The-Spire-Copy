using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSystem : MonoSingleton<HeroSystem>
{
    [field: SerializeField] public HeroView HeroView { get; set; }

    private void OnEnable()
    {
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPreReaction, ReactionTiming.PRE);
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
    }

    private void OnDisable()
    {
        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPreReaction, ReactionTiming.PRE);
        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
    }

    public void Setup(HeroData heroData)
    {
        HeroView.Setup(heroData);
    }
    
    /// <summary>
    /// 敌人回合前事件
    /// </summary>
    /// <param name="enemyTurnGA"></param>
    private void EnemyTurnPreReaction(EnemyTurnGA enemyTurnGA)
    {
        //清空玩家手中所有手牌
        DiscardAllCardGA discardAllCardGA = new DiscardAllCardGA();
        ActionSystem.Instance.AddReaction(discardAllCardGA);
    }
    
    /// <summary>
    /// 敌人回合结束事件
    /// </summary>
    /// <param name="enemyTurnGA"></param>
    private void EnemyTurnPostReaction(EnemyTurnGA enemyTurnGA)
    {
        //结算负面状态伤害 
        int burnStacks = HeroView.GetStatusEffectStacks(StatusEffectType.BURN);
        if (burnStacks > 0)
        {
            ApplyBurnGA applyBurnGA = new ApplyBurnGA(burnStacks, HeroView);
            ActionSystem.Instance.AddReaction(applyBurnGA);
        }
        //设置玩家可抽卡数量为5
        DrawCardsGA drawCardsGA = new DrawCardsGA(5);
        ActionSystem.Instance.AddReaction(drawCardsGA);
    }
}