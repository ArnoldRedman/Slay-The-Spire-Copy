using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeroView : CombatantView
{


    public void Setup(HeroData heroData)
    {
        SetupBase(heroData.Health, heroData.Image);
    }
}
