using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatSystem : MonoBehaviour
{
    private void OnEnable()
    {
        ActionSystem.AttachPerformer<IncreaseStatsGA>(IncreaseStatsPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<IncreaseStatsGA>();
    }

    private IEnumerator IncreaseStatsPerformer(IncreaseStatsGA increaseStatsGA)
    {
        int attack = increaseStatsGA.AttackIncreaseAmount;
        int health = increaseStatsGA.HealthIncreaseAmount;
        yield return increaseStatsGA.Target.IncreaseAttackAndHealth(attack, health);
    }
}
