using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    [SerializeField] private GameObject damageVFX;
    
    
    private void OnEnable()
    {
        ActionSystem.AttachPerformer<DealDamageGA>(DealDamagePerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<DealDamageGA>();
    }

    /// <summary>
    /// 对单位造成伤害 玩家 敌人 共用受击系统
    /// </summary>
    /// <param name="dealDamageGA"></param>
    /// <returns></returns>
    private IEnumerator DealDamagePerformer(DealDamageGA dealDamageGA)
    {
        foreach (var target in dealDamageGA.Targets)
        {
            target.Damage(dealDamageGA.Amount);
            //生成受击特效
            Instantiate(damageVFX, target.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.15f);
            if (target.CurrentHealth <= 0)
            {
                //敌人血量被打的小于0
                if (target is EnemyView enemyView)
                {
                    KillEnemyGA killEnemyGA = new KillEnemyGA(enemyView);
                    ActionSystem.Instance.AddReaction(killEnemyGA);
                }
                else//玩家血量被打的小于0
                {
                    
                }
            }
        }
    }
}
