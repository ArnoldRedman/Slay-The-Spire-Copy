using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchSetupSystem : MonoBehaviour
{
    [SerializeField] private HeroData heroData;
    [SerializeField] private PerkData perkData;
    [SerializeField] private List<EnemyData> enemyDatas;

    private void Start()
    {
        //初始化角色图标等视图数据
        HeroSystem.Instance.Setup(heroData);
        //根据选择的角色初始化卡组
        CardSystem.Instance.SetUp(heroData.Deck);
        EnemySystem.Instance.Setup(enemyDatas);
        PerkSystem.Instance.AddPerk(new Perk(perkData));
        DrawCardsGA drawCardsGA = new DrawCardsGA(5);
        ActionSystem.Instance.Perform(drawCardsGA);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EnemySystem.Instance.Setup(enemyDatas);
        }
    }
}
