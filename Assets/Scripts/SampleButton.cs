﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleButton : MonoBehaviour {

    public Button buttonComponent;
    public Text nameLabel;
    public Image iconImage;
    public Text priceText;


    private Item item;
    private ShopScrollList scrollList;

    void Start()
    {
        buttonComponent.onClick.AddListener(HandleClick);
    }

    public void Setup(Item currentItem, ShopScrollList currentScrollList)
    {
        item = currentItem;
        nameLabel.text = item.name;
        iconImage.sprite = item.icon;
        priceText.text = item.price.ToString();
        scrollList = currentScrollList;
    }

    public void HandleClick()
    {
        scrollList.TryBuyItem(item);
    }
}
