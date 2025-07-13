using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class EnemyBoardView : MonoBehaviour
{
    //敌人视图位置
    [SerializeField] private List<Transform> slots = new List<Transform>();
    public List<EnemyView> EnemyViews { get; private set; } = new List<EnemyView>();



    /// <summary>
    /// 向敌人链表中添加敌人 并生成视图
    /// </summary>
    /// <param name="enemyData"></param>
    public void AddEnemy(EnemyData enemyData)
    {
        Transform slot = slots[EnemyViews.Count];
        EnemyView enemyView = EnemyViewCreator.Instance.CreateEnemyView(enemyData,slot.position,slot.rotation);
        enemyView.transform.parent = slot;
        EnemyViews.Add(enemyView);
    }

    /// <summary>
    /// 敌人被打死
    /// </summary>
    /// <param name="enemyView"></param>
    /// <returns></returns>
    public IEnumerator RemoveEnemy(EnemyView enemyView)
    {
        EnemyViews.Remove(enemyView);
        Tween tween = Tween.Scale(enemyView.transform, Vector3.zero, 0.25f);
        yield return tween;
        Destroy(enemyView.gameObject);
    }
}
