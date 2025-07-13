using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class CardSystem : MonoSingleton<CardSystem>
{
    [SerializeField] private HandView handView;
    [Tooltip("抽牌堆视觉位置")]
    [SerializeField] private Transform drawPilePoint;
    [Tooltip("弃牌堆视觉位置")]
    [SerializeField] private Transform discardPilePoint;
    private readonly List<Card> drawPile = new List<Card>();//抽牌堆
    private readonly List<Card> discardPile = new List<Card>();//弃牌堆
    private readonly List<Card> hand = new List<Card>();//手牌

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<DrawCardsGA>(DrawCardPerformer);
        ActionSystem.AttachPerformer<DiscardAllCardGA>(DiscardAllCardPerformer);
        ActionSystem.AttachPerformer<PlayCardGA>(PlayCardPerformer);
        
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<DrawCardsGA>();
        ActionSystem.DetachPerformer<DiscardAllCardGA>();
        ActionSystem.DetachPerformer<PlayCardGA>();
        
    }

    /// <summary>
    /// 往抽牌区初始化玩家可用卡牌
    /// </summary>
    /// <param name="deckData"></param>
    public void SetUp(List<CardData> deckData)
    {
        foreach (var cardData in deckData)
        {
            Card card = new Card(cardData);
            drawPile.Add(card);
        }
    }

    /// <summary>
    /// 玩家使用卡牌
    /// </summary>
    /// <param name="playCardGA"></param>
    /// <returns></returns>
    private IEnumerator PlayCardPerformer(PlayCardGA playCardGA)
    {
        //从数据和显示中都移除当前使用掉的牌 同时记录使用的是什么
        hand.Remove(playCardGA.Card);
        CardView cardView = handView.RemoveCard(playCardGA.Card);
        yield return DiscardCard(cardView);

        //消耗能量
        SpendManaGA spendManaGA = new SpendManaGA(playCardGA.Card.Mana);
        ActionSystem.Instance.AddReaction(spendManaGA);

        if (playCardGA.Card.ManualTargetEffect != null)
        {
            PerformEffectGA performEffectGA = new PerformEffectGA(playCardGA.Card.ManualTargetEffect,new List<CombatantView>(){ playCardGA.ManualTarget });
            ActionSystem.Instance.AddReaction(performEffectGA);
        }
        
        //卡牌的使用效果
        foreach (var effectWrapper in playCardGA.Card.OtherEffects)
        {
            List<CombatantView> targets = effectWrapper.TargetMode.GetTargets();
            PerformEffectGA performEffectGA = new PerformEffectGA(effectWrapper.Effect, targets);
            ActionSystem.Instance.AddReaction(performEffectGA);
        }
    }
    
    /// <summary>
    /// 抽卡事件
    /// </summary>
    /// <param name="drawCardsGA"></param>
    /// <returns></returns>
    private IEnumerator DrawCardPerformer(DrawCardsGA drawCardsGA)
    {
        // 计算在弃牌堆的牌重新填满牌组之前 我们实际可以从牌组抽出多少张牌
        int actualAmount = Mathf.Min(drawCardsGA.Amount, drawPile.Count);
        //计算未抽取的卡牌数量
        int notDrawnAmount = drawCardsGA.Amount - actualAmount;
        //抽取实际数量的卡牌
        for (int i = 0; i < actualAmount; i++)
        {
            yield return DrawCard();
        }

        //存在未抽取的卡牌
        if (notDrawnAmount > 0)
        {
            RefillDeck();
            for (int i = 0; i < notDrawnAmount; i++)
            {
                yield return DrawCard();
            }
        }
    }

    /// <summary>
    /// 丢弃所有卡牌事件
    /// </summary>
    /// <param name="discardAllCardGA"></param>
    /// <returns></returns>
    private IEnumerator DiscardAllCardPerformer(DiscardAllCardGA discardAllCardGA)
    {
        //将手牌逐个移除视野启动弃牌动画
        foreach (var card in hand)
        {
            CardView cardView = handView.RemoveCard(card);
            yield return DiscardCard(cardView);
        }
        //清空手牌数据
        hand.Clear();
    }

    /// <summary>
    /// 弃牌
    /// </summary>
    /// <param name="cardView"></param>
    /// <returns></returns>
    private IEnumerator DiscardCard(CardView cardView)
    {
        //将需要丢弃的牌数据添加到弃牌堆
        discardPile.Add(cardView.Card);
        Tween.Scale(cardView.transform, Vector3.zero, 0.15f);
        var tween = Tween.Position(cardView.transform, discardPilePoint.position, 0.15f);
        yield return tween;
        Destroy(cardView.gameObject);
    }

    /// <summary>
    /// 玩家抽卡协程
    /// </summary>
    /// <returns></returns>
    private IEnumerator DrawCard()
    {
        //从抽牌堆随机抽取一张 删除原本抽牌堆中的牌
        Card card = drawPile.Draw();
        //增加到手牌链表
        hand.Add(card);
        //增加抽到手中的视觉效果
        CardView cardView = CardViewCreator.Instance.CreateCardView(card, drawPilePoint.position, drawPilePoint.rotation);
        //等待卡牌到抽到手中的动画完成
        yield return handView.AddCard(cardView);
    }

    /// <summary>
    /// 重新填充卡组
    /// </summary>
    private void RefillDeck()
    {
        //把弃牌堆的卡 随机重新填充到抽牌区 完成洗牌
        drawPile.AddRange(discardPile);
        //清空原本的弃牌堆数据
        discardPile.Clear();
    }
}
