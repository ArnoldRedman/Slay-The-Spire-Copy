using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    private void OnEnable()
    {
        ActionSystem.AttachPerformer<DealDamageGA>(DealDamagePerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<DealDamageGA>();
    }

    private IEnumerator DealDamagePerformer(DealDamageGA dealDamageGA)
    {
        int damageAmount = dealDamageGA.Amount;
        //Vector2 knifeStart = knife.transform.position;
        //var tween = Tween.Position(knife.transformm, health.transform.position, 0.25f);
        //yield return tween;
        //Tween.Position(knife.transform, knifeStart, 0.5f);
        //yield return health.ReduceHealth(damageAmount);;
        yield return null;
    }
}
