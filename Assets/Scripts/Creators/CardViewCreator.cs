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

    public CardView CreateCardView(Card card, Vector3 position, Quaternion rotation)
    {
        CardView cardView = Instantiate(cardViewPrefab, position, rotation);
        cardView.transform.localScale = Vector3.zero;
        Tween.Scale(cardView.transform, Vector3.one, 0.15f);
        cardView.Setup(card);
        return cardView;
    }
}
