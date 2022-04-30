using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButtonStorage : MonoBehaviour
{
    [SerializeField] private GameObject crownImage;
    [SerializeField] private GameObject buttonMax;
    private ItemConfig itemConfig;
    private ScrollBoxScript scrollBoxScript;
    private Button button;
    private Transform inventoryItem;
    private int price = 1;

    private void Awake()
    {
        button = GetComponent<Button>();
   }

    /// <summary>
    /// Set the certain itemConfig for this button
    /// </summary>
    /// <param name="newItemConfig"></param>
    public void SetItemConfig(ItemConfig newItemConfig)
    {
        itemConfig = newItemConfig;
        if (itemConfig.GetIsPremium()) crownImage.SetActive(true);
    }

    public void SetPriceOnTable(int newPrice)
    {
        price = newPrice;
    }

    public int GetPriceOnTable()
    {
        return price;
    }

    public void SetScrollBoxScript(ScrollBoxScript newScrollBoxScript)
    {
        scrollBoxScript = newScrollBoxScript;
    }

    public void GetParentComponents(Transform newInventoryItem, ItemConfig itemConfig)
    {
        SetItemConfig(itemConfig);
        inventoryItem = newInventoryItem;
        scrollBoxScript = transform.parent.parent.parent.gameObject.GetComponent<ScrollBoxScript>();
    }

    public void TradeItem()
    {
        button.interactable = false;
        scrollBoxScript.TradeCertainItem(itemConfig, inventoryItem.gameObject, gameObject, false);
    }

    public void TradeItemByMaxNumber()
    {
        scrollBoxScript.TradeCertainItem(itemConfig, inventoryItem.gameObject, gameObject, true);
    }

    public void ShowMaxButton()
    {
        buttonMax.SetActive(true);
    }
}
