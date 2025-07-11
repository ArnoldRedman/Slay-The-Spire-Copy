using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    private void OnEnable()
    {
        ActionSystem.SubscribeReaction<DrawCardsGA>(DrawCardReaction, ReactionTiming.PRE);
    }

    private void OnDisable()
    {
        ActionSystem.UnsubscribeReaction<DrawCardsGA>(DrawCardReaction, ReactionTiming.PRE);
    }

    private void DrawCardReaction(DrawCardsGA drawCardsGa)
    {
        DealDamageGA dealDamageGA = new DealDamageGA(3);
        ActionSystem.Instance.AddReaction(dealDamageGA);
    }
}
