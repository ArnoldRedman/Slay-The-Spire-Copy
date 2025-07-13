using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 消耗能量
/// </summary>
public class SpendManaGA : GameAction
{
    public int Amount {get; set; }

    public SpendManaGA(int amount)
    {
        Amount = amount;
    }
}
