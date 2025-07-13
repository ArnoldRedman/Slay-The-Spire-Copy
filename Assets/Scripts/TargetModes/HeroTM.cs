using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroTM : TargetMode
{
    public override List<CombatantView> GetTargets()
    {
        List<CombatantView> targets = new List<CombatantView>()
        {
            HeroSystem.Instance.HeroView
        };
        
        return targets;
    }
}
