using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllEnemiesTM : TargetMode
{
    public override List<CombatantView> GetTargets()
    {
        return new List<CombatantView>(EnemySystem.Instance.Enemies);
    }
}
