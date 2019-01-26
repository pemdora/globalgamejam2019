using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Item
{
    public string itemName;
    public Sprite icon;
    public float price = 1;
}

public class ShopScrollList : MonoBehaviour {

    public List<Item> itemList;
    public Transform contentPanel;
    public Text moneyUI;
    public SimpleObjectPool buttonObjectPool;


    void Start () {
        RefreshDisplay();
	}

    public void RefreshDisplay()
    {
        //myGoldDisplay.text = "Gold: " + gold.ToString();
        //RemoveButtons();
        AddButtons();
    }

    private void AddButtons()
    {
        foreach (Item item in itemList)
        {
            GameObject newButton = buttonObjectPool.GetObject();
            newButton.transform.SetParent(contentPanel);

            SampleButton sampleButton = newButton.GetComponent<SampleButton>();
            sampleButton.Setup(item, this);
        }
    }

    public void BuyItem(int price)
    {
        //PlayerManager.instance.UpdateMoney();
    }
}
