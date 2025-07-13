using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualTargetSystem : MonoSingleton<ManualTargetSystem>
{
    [SerializeField] private ArrowView arrowView;
    [SerializeField] private LayerMask targetLayerMask;



    public void StartTargeting(Vector3 startPosition)
    {
        arrowView.gameObject.SetActive(true);
        arrowView.SetupArrow(startPosition);
    }

    /// <summary>
    /// 判断结束是否使用在了敌人身上
    /// </summary>
    /// <param name="endPosition"></param>
    /// <returns></returns>
    public EnemyView EnemyTargeting(Vector3 endPosition)
    {
        arrowView.gameObject.SetActive(false);
        if (Physics.Raycast(endPosition, Vector3.forward, out RaycastHit hit, 10f, targetLayerMask) 
            && hit.collider != null
            && hit.transform.TryGetComponent(out EnemyView enemyView))
        {
            return enemyView;
        }
        
        return null;
    }
}
