using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class EnemySystem : MonoSingleton<EnemySystem>
{
    [SerializeField] private EnemyBoardView enemyBoardView;
    public List<EnemyView> Enemies => enemyBoardView.EnemyViews;
    
    
    private void OnEnable()
    {
        ActionSystem.AttachPerformer<EnemyTurnGA>(EnemyTurnPerformer);
        ActionSystem.AttachPerformer<AttackHeroGA>(AttackHeroPerformer);
        ActionSystem.AttachPerformer<KillEnemyGA>(KillEnemyPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<EnemyTurnGA>();
        ActionSystem.DetachPerformer<AttackHeroGA>();
        ActionSystem.DetachPerformer<KillEnemyGA>();
    }

    /// <summary>
    /// 初始化本关敌人 
    /// </summary>
    /// <param name="enemyDatas"></param>
    public void Setup(List<EnemyData> enemyDatas)
    {
        foreach (var enemyData in enemyDatas)
        {
            enemyBoardView.AddEnemy(enemyData);
        }
    }

    /// <summary>
    /// 轮到敌方回合
    /// </summary>
    /// <param name="enemyTurnGA"></param>
    /// <returns></returns>
    private IEnumerator EnemyTurnPerformer(EnemyTurnGA enemyTurnGA)
    {
        Debug.Log("敌方回合");

        foreach (var enemy in enemyBoardView.EnemyViews)
        {
            AttackHeroGA attackHeroGA = new AttackHeroGA(enemy);
            ActionSystem.Instance.AddReaction(attackHeroGA);
        }
        
        yield return null;
        Debug.Log("敌方回合结束");
    }

    /// <summary>
    /// 敌人攻击Performer
    /// </summary>
    /// <param name="attackHeroGA"></param>
    /// <returns></returns>
    private IEnumerator AttackHeroPerformer(AttackHeroGA attackHeroGA)
    {
        //攻击动画效果
        EnemyView attacker = attackHeroGA.Attacker;
        Tween tween = Tween.PositionX(attacker.transform, attacker.transform.position.x - 1f, 0.15f);
        yield return tween;
        Tween.PositionX(attacker.transform, attacker.transform.position.x + 1f, 0.25f);
        //造成伤害
        DealDamageGA dealDamageGA = new DealDamageGA(attacker.AttackPower,
            new List<CombatantView>() { HeroSystem.Instance.HeroView });
        ActionSystem.Instance.AddReaction(dealDamageGA);
    }

    private IEnumerator KillEnemyPerformer(KillEnemyGA killEnemyGA)
    {
        yield return enemyBoardView.RemoveEnemy(killEnemyGA.EnemyView);
    }
}
