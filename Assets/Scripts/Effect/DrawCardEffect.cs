using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 抽卡卡牌    
/// </summary>
public class DrawCardEffect : Effect
{
    [SerializeField] private int drawAmount;
    
    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        DrawCardsGA drawCardsGA = new DrawCardsGA(drawAmount);
        return drawCardsGA; 
    }
}
