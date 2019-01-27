using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Item
{
    public string name;
    public Sprite icon;
    public Sprite type;
    public int price = 0;
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

    public void TryBuyItem(Item item)
    {
        if(PlayerManager.instance.GetMoney() >= item.price)
        {
            PlayerManager.instance.UpdateMoney(-item.price);
            LevelManager.instance.SpawnObject(item.name);
            Debug.Log("buy : " + item.name);
        }
    }
    
}
