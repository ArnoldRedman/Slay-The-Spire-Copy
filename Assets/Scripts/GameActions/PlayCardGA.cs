using UnityEngine;

public class PlayCardGA : GameAction
{
    public Card Card {get; set;}
    
    /// <summary>
    /// 存储当前想要打出的牌
    /// </summary>
    /// <param name="card"></param>
    public PlayCardGA(Card card)
    {
        Card = card;
    }
}
