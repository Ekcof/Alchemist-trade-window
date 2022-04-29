using System.Collections.Generic;
using UnityEngine;

public class ItemDataStorage : MonoBehaviour
{
    [SerializeField] private string personName;
    [SerializeField] private int money;
    [SerializeField] private GameObject inventory; //Gameobject of inventory of trader
    [SerializeField] private int tradeCoefficient = 100; //Coefficient of items to sell
    [SerializeField] private int premiumMoneyRate = 200; //Money rate which expose you premium items if you have more money
    private ItemObject itemObject;
    List<ItemObject> itemList = new List<ItemObject>();

    /// <summary>
    /// Get the person name fo UI
    /// </summary>
    /// <returns></returns>
    public string GetPersonName()
    {
        return personName;
    }

    /// <summary>
    /// Set person name which will be used in UI
    /// </summary>
    /// <param name="newName"></param>
    public void SetPersonName(string newName)
    {
        personName = newName;
    }

    /// <summary>
    /// Get the current amount of money
    /// </summary>
    /// <returns></returns>
    public int GetMoney()
    {
        return money;
    }

    /// <summary>
    /// Add certain amount of money
    /// </summary>
    /// <param name="addMoney"></param>
    public void AddMoney(int addMoney)
    {
        int newMoney = money + addMoney;
        if (newMoney < 0)
        {
            money = 0;
        }
        else
        {
            money = newMoney;
        }
    }

    /// <summary>
    /// Add item to inventory with a specific number
    /// </summary>
    /// <param name="title"></param>
    /// <param name="itemNumber"></param>
    public void AddItem(string title, int itemNumber)
    {
        if (itemList.Count > 0)
        {
            foreach (ItemObject item in itemList)
            {
                if (item.Title == title)
                {
                    int newItemNumber = item.ItemNumber + itemNumber;
                    if (newItemNumber <= 0)
                    {
                        RemoveAllItemsByTitle(title);
                    }
                    else
                    {
                        item.ItemNumber = newItemNumber;
                    }
                    break;
                }
            }
        }
        else
        {
            if (itemNumber > 0)
            {
                ItemObject newItem = new ItemObject(title, itemNumber, 0, 0f, null);
                itemList.Add(newItem);
            }
        }
    }

    /// <summary>
    /// Remove all items of certain type
    /// </summary>
    /// <param name="title"></param>
    public void RemoveAllItemsByTitle(string title)
    {
        if (itemList.Count > 0)
        {
            foreach (ItemObject item in itemList)
            {
                if (item.Title == title)
                {
                    itemList.Remove(item);
                    break;
                }
            }
        }
    }

    public List<ItemObject> GetItemList()
    {
        return itemList;
    }

    public int GetTradeCoefficient()
    {
        return tradeCoefficient;
    }

    public void SetTradeCoefficient(int NewTradeCoefficient)
    {
        tradeCoefficient = NewTradeCoefficient;
    }

    /// <summary>
    /// Get the rate of money after which yo can by premium items
    /// </summary>
    /// <returns></returns>
    public int GetPremiumMoneyRate()
    {
        return premiumMoneyRate;
    }

    /// <summary>
    /// Set the rate of money after which yo can by premium items
    /// </summary>
    /// <param name="newRate"></param>
    public void SetPremiumMoneyRate(int newRate)
    {
        premiumMoneyRate = newRate;
    }
}
