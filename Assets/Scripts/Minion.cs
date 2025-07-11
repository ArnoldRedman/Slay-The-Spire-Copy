using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{
    private int attack;
    private int health;
    
    private void OnEnable()
    {
        ActionSystem.SubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.POST);
    }

    private void OnDisable()
    {
        ActionSystem.UnsubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.POST);
    }

    private void DealDamageReaction(DealDamageGA dealDamageGA)
    {
        IncreaseStatsGA increaseStatsGA = new IncreaseStatsGA(this, dealDamageGA.Amount, dealDamageGA.Amount);
        ActionSystem.Instance.AddReaction(increaseStatsGA);
    }

    public IEnumerator IncreaseAttackAndHealth(int attack, int health)
    {
        this.attack += attack;
        this.health += health;
        //attackText.text = this.attack.ToString();
        yield return null;
    }
}
