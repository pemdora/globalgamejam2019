using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleButton : MonoBehaviour {

    public Button buttonComponent;
    public Image iconImage;
    public Image priceImage;
    public Text priceText;

    private Item item;
    private ShopScrollList scrollList;

    void Start()
    {
        transform.localScale = new Vector3(1,1,1);
        buttonComponent.onClick.AddListener(HandleClick);
    }

    public void Setup(Item currentItem, ShopScrollList currentScrollList)
    {
        item = currentItem;
        iconImage.sprite = item.icon;
        priceImage.sprite = item.type;
        priceText.text = item.price.ToString();
        scrollList = currentScrollList;
    }

    public void HandleClick()
    {
        scrollList.TryBuyItem(item);
    }
}
