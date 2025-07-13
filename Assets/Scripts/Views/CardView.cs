using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardView : MonoBehaviour
{
    public Card Card { get; private set; }
    
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text mana;
    [SerializeField] private SpriteRenderer imageSR;
    [SerializeField] private GameObject wrapper;
    [SerializeField] private LayerMask dropLayer;//卡牌使用区域的层级
    
    private Vector3 dragStartPosition;
    private Quaternion dragStartRotation;

    /// <summary>
    /// 初始化这张卡牌的信息
    /// </summary>
    /// <param name="card">card类</param>
    public void Setup(Card card)
    {
        Card = card;
        title.text = card.Title;
        description.text = card.Description;
        mana.text = card.Mana.ToString();
        imageSR.sprite = card.Image;
    }
    
    /// <summary>
    /// 选中卡牌放大效果
    /// </summary>
    private void OnMouseEnter()
    {
        if (!Interactions.Instance.PlayerCanHover())
            return;
        
        wrapper.SetActive(false);
        Vector3 pos = new Vector3(transform.position.x, -2, 0);
        CardViewHoverSystem.Instance.Show(Card, pos);
    }

    /// <summary>
    /// 鼠标离开卡牌复原
    /// </summary>
    private void OnMouseExit()
    {
        if (!Interactions.Instance.PlayerCanHover())
            return;
        
        CardViewHoverSystem.Instance.Hide();
        wrapper.SetActive(true);
    }
    
    /// <summary>
    /// 鼠标按下卡牌 进行移动前的准备
    /// </summary>
    private void OnMouseDown()
    {
        if (!Interactions.Instance.PlayerCanInteract())
            return;

        if (Card.ManualTargetEffect != null)
        {
            ManualTargetSystem.Instance.StartTargeting(transform.position);
        }
        else
        {
            Interactions.Instance.PlayerIsDragging = true;
            wrapper.SetActive(true);
            CardViewHoverSystem.Instance.Hide();
            dragStartPosition = transform.position;
            dragStartRotation = transform.rotation;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.position = MouseUtil.GetMousePositionInWorldSpace(-1);
        }
    }

    /// <summary>
    /// 拖动当前选中卡牌
    /// </summary>
    private void OnMouseDrag()
    {
        if (!Interactions.Instance.PlayerCanInteract())
            return;

        if (Card.ManualTargetEffect != null)
            return;
        
        transform.position = MouseUtil.GetMousePositionInWorldSpace(-1);
    }

    /// <summary>
    /// 对选中的卡牌进行落子判断
    /// </summary>
    private void OnMouseUp()
    {
        if (!Interactions.Instance.PlayerCanInteract())
            return;

        if (Card.ManualTargetEffect != null)
        {
            EnemyView target = ManualTargetSystem.Instance.EnemyTargeting(MouseUtil.GetMousePositionInWorldSpace(-1));
            if (target != null && ManaSystem.Instance.HasEnoughMana(Card.Mana))
            {
                PlayCardGA playCardGA = new PlayCardGA(Card, target);
                ActionSystem.Instance.Perform(playCardGA);
            }
        }
        else
        {
            if (ManaSystem.Instance.HasEnoughMana(Card.Mana) 
                && Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit, 10f, dropLayer))
            {
                PlayCardGA playCardGA = new PlayCardGA(Card);//以显示卡牌作为中间数据 读取出去新的卡牌数据 获取到当前选中卡牌数据
                ActionSystem.Instance.Perform(playCardGA);
            }
            else//没放在有效区域回到手牌区域
            {
                transform.position = dragStartPosition;
                transform.rotation = dragStartRotation;
            }
            Interactions.Instance.PlayerIsDragging = false;
        }
    }
}
