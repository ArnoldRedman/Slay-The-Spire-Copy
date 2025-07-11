using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class CardViewCreator : MonoSingleton<CardViewCreator>
{
    [SerializeField] private CardView cardViewPrefab;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    /// <summary>
    /// 生成新的卡牌
    /// </summary>
    /// <param name="card">卡牌信息card类</param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <returns>生成的卡牌实例</returns>
    public CardView CreateCardView(Card card, Vector3 position, Quaternion rotation)
    {
        //简单实例化
        CardView cardView = Instantiate(cardViewPrefab, position, rotation);
        cardView.transform.localScale = Vector3.zero;
        //从0到1的动画效果
        Tween.Scale(cardView.transform, Vector3.one, 0.15f);
        //给实例化出来的卡牌赋值
        cardView.Setup(card);
        //返回使用信息创造的实例卡牌
        return cardView;
    }
}
