using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBoxScript : MonoBehaviour
{
    [SerializeField] private GameObject trader; //Gameobject of trader or player whos inventory is exposed
    [SerializeField] private GameObject counterPart; //Gameobject of counterpart trader
    [SerializeField] private Transform content; //Gameobject of UI content in ScrollRect
    [SerializeField] private GameObject inventory; //Gameobject of inventory of trader
    [SerializeField] private GameObject scrollBoxItem; //UI item prefab
    [SerializeField] private Transform scrollBar; //UI ScrollBar
    [SerializeField] private Text moneyText; // UI money text
    [SerializeField] private Text nameText; // UI name of the trader
    [SerializeField] private ScrollBoxScript counterScrollBoxScript; // ScrollBoxScript of counterpart
    [SerializeField] private Text infoText; // UI info text below the trading table
    private ItemDataStorage itemDataStorage; // ItemDataStorage script of the trader
    private int money;
    private int contentChildrenCount;
    private bool refreshInProcess;
    private float contentDeltaSize;
    private float scrollBoxItemDeltaSize;
    private float contentParentRectSize;
    private float currentScrollBoxDeltaSize = 0f;
    private RectTransform contentRectTransform;
    private RectTransform scrollBoxItemRectTransform;
    private RectTransform contentParentRectTransform;
    private bool isPlayer;
    private ItemDataStorage counterPartStorage;

    private void Awake()
    {
        itemDataStorage = trader.GetComponent<ItemDataStorage>();
        contentParentRectTransform = content.parent.GetComponent<RectTransform>();
        contentRectTransform = content.GetComponent<RectTransform>();
        scrollBoxItemRectTransform = scrollBoxItem.GetComponent<RectTransform>();
        contentParentRectSize = contentParentRectTransform.rect.height;
        contentDeltaSize = contentRectTransform.sizeDelta.y;
        scrollBoxItemDeltaSize = scrollBoxItemRectTransform.sizeDelta.y;
        money = itemDataStorage.GetMoney();
        nameText.text = itemDataStorage.GetPersonName();
        isPlayer = (trader.name == "Player" || trader.CompareTag("Player"));
        counterPartStorage = counterPart.GetComponent<ItemDataStorage>();
        Refresh();
    }

    private void Update()
    {
        if (refreshInProcess)
        {
            contentChildrenCount = content.transform.childCount;
            if (contentChildrenCount == 0)
            {
                refreshInProcess = false;
                FillTheBox();
            }
        }
    }

    /// <summary>
    /// Refresh the scrollbox
    /// </summary>
    public void Refresh()
    {
        refreshInProcess = true;
        Transform[] allChildren = content.GetComponentsInChildren<Transform>();
        RefreshMoneyText(moneyText, itemDataStorage);
        foreach (Transform transform in allChildren)
        {
            if (transform != content.transform) Destroy(transform.gameObject);
        }
    }

    /// <summary>
    /// Refresh money number text
    /// </summary>
    /// <param name="mText"></param>
    /// <param name="dataStorage"></param>
    public void RefreshMoneyText(Text mText,ItemDataStorage dataStorage)
    {
        mText.text = "Money: " + dataStorage.GetMoney();
    }

    /// <summary>
    /// Add the certain item to the scrollbox
    /// </summary>
    /// <param name="inventoryItem"></param>
    /// <param name="itemConfig"></param>
    public void AddScrollBoxItem(Transform inventoryItem, ItemConfig itemConfig)
    {
        GameObject newScrollBoxItem = Instantiate(scrollBoxItem, new Vector3(0, 0, 0), Quaternion.identity);
        newScrollBoxItem.transform.SetParent(content, false);

        ItemButtonStorage itemButtonStorage = newScrollBoxItem.GetComponent<ItemButtonStorage>();
        itemButtonStorage.GetParentComponents(inventoryItem, itemConfig);
        itemButtonStorage.SetPriceOnTable((itemConfig.GetPrice() * itemDataStorage.GetTradeCoefficient()) / 100);
        if (itemConfig.GetNumber() > 1)
        {
            itemButtonStorage.ShowMaxButton();
        }

        GameObject imageGO = newScrollBoxItem.transform.GetChild(0).gameObject;
        imageGO.GetComponent<Image>().sprite = itemConfig.GetSprite();
        Text textGO = imageGO.transform.GetChild(0).GetComponent<Text>();

        int itemCount = itemConfig.GetNumber();
        string itemTitle = itemConfig.GetTitle();
        if (itemCount > 1)
        {
            textGO.text = itemTitle + " x" + itemCount;
        }
        else
        {
            textGO.text = itemTitle;
        }
        RectTransform rectTransform = newScrollBoxItem.GetComponent<RectTransform>();
        GameObject numberText = imageGO.transform.Find("NumberText").gameObject;
        numberText.GetComponent<Text>().text = " " + itemButtonStorage.GetPriceOnTable();
        int childrenCount = content.childCount;
        if (childrenCount > 1)
        {
            newScrollBoxItem.transform.localPosition = new Vector3(newScrollBoxItem.transform.localPosition.x, newScrollBoxItem.transform.localPosition.y - rectTransform.sizeDelta.y * (childrenCount - 1), newScrollBoxItem.transform.localPosition.z);
        }
    }

    /// <summary>
    /// Trade the certain item from a UI button
    /// </summary>
    /// <param name="itemConfig"></param>
    /// <param name="buttonGameObject"></param>
    /// <param name="buttonObject"></param>
    /// <result></result>
    public void TradeCertainItem(ItemConfig itemConfig, GameObject buttonGameObject, GameObject buttonObject, bool tradeMaximum)
    {
        money = itemDataStorage.GetMoney();
        ItemButtonStorage itemButtonStorage = buttonObject.GetComponent<ItemButtonStorage>();
        int sum = itemButtonStorage.GetPriceOnTable();
        if (itemConfig.GetNumber() > 1)
        {
            itemButtonStorage.ShowMaxButton();
        }

        int numberToSell = 1;
        int counterMoney = counterPartStorage.GetMoney();
        if (tradeMaximum)
        {
            numberToSell = GetMaximumAvailableItemNumber(itemConfig, sum, counterMoney);
            sum *= numberToSell;
        }
        if (counterMoney >= sum && sum != 0)
        {
            Transform inventoryCounterpart = counterPart.transform.Find("Inventory");
            if (inventoryCounterpart != null)
            {
                TradeItemWithCounterpart(sum, inventoryCounterpart, itemConfig, numberToSell, buttonGameObject);
            }
            counterPartStorage.AddMoney(sum * -1);
            itemDataStorage.AddMoney(sum);
            itemConfig.SetNumber(itemConfig.GetNumber() - numberToSell);
            Refresh();
            counterScrollBoxScript.Refresh();
            if (isPlayer)
            {
                infoText.text = "You have sold" + " " + itemConfig.GetTitle();
            }
            else
            {
                infoText.text = "You have bought" + " " + itemConfig.GetTitle();
            }
        }
        else
        {
            if (isPlayer)
            {
                infoText.text = counterPartStorage.GetPersonName() + " has not enough money!";
            }
            else
            {
                infoText.text = "You has not enough money!";
            }
        }
    }

    /// <summary>
    /// Remove item from trader and add it to counterpart's inventory
    /// </summary>
    /// <param name="sum"></param>
    /// <param name="inventoryCounterpart"></param>
    /// <param name="itemConfig"></param>
    /// <param name="numberToSell"></param>
    /// <param name="buttonGameObject"></param>
    public void TradeItemWithCounterpart(int sum, Transform inventoryCounterpart, ItemConfig itemConfig, int numberToSell, GameObject buttonGameObject)
    {
        Transform itemTransform = inventoryCounterpart.Find(itemConfig.GetTitle());
        if (itemTransform != null)
        {
            ItemConfig counterItemConfig = itemTransform.GetComponent<ItemConfig>();
            counterItemConfig.SetNumber(counterItemConfig.GetNumber() + numberToSell);
        }
        else
        {
            GameObject newItem = Instantiate(buttonGameObject, new Vector3(0, 0, 0), Quaternion.identity);
            newItem.name = itemConfig.GetTitle();
            newItem.transform.SetParent(inventoryCounterpart, false);
            ItemConfig newItemConfig = newItem.GetComponent<ItemConfig>();
            newItemConfig.SetNumber(numberToSell);
        }
    }

    /// <summary>
    /// Fill the ScrollBox with items from trader's inventory
    /// </summary>
    private void FillTheBox()
    {
        int moneyRate = -1;
        if (!isPlayer) moneyRate = counterPartStorage.GetPremiumMoneyRate();
        currentScrollBoxDeltaSize = 0f;
        Transform[] allInventoryItemTransform = inventory.GetComponentsInChildren<Transform>();
        int trueScrollBoxItemsCount = 0;
        foreach (Transform transform in allInventoryItemTransform)
        {
            currentScrollBoxDeltaSize += scrollBoxItemDeltaSize;
            ItemConfig itemConfig = transform.gameObject.GetComponent<ItemConfig>();
            if (itemConfig != null)
            {
                if (isPlayer || !itemConfig.GetIsPremium() || (moneyRate <= counterPartStorage.GetMoney() && itemConfig.GetIsPremium()))
                {
                    AddScrollBoxItem(transform, itemConfig);
                    trueScrollBoxItemsCount += 1;
                }
            }
        }

        currentScrollBoxDeltaSize = scrollBoxItemDeltaSize * trueScrollBoxItemsCount;

        if (currentScrollBoxDeltaSize > contentParentRectSize)
        {
            scrollBar.GetChild(0).gameObject.SetActive(true);
            contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, currentScrollBoxDeltaSize - contentParentRectSize);
        }
        else
        {
            scrollBar.GetChild(0).gameObject.SetActive(false);
        }
        infoText.text = "Let's trade!";
    }

    /// <summary>
    /// Get the maximum number of item trader can afford
    /// </summary>
    /// <param name="itemConfig"></param>
    /// <param name="sum"></param>
    /// <param name="counterMoney"></param>
    /// <returns></returns>
    private int GetMaximumAvailableItemNumber(ItemConfig itemConfig, int sum, int counterMoney)
    {
        int maxNumber = itemConfig.GetNumber();

        for (int maxSum = sum * maxNumber; maxSum > counterMoney; maxSum -= sum)
        {
            maxNumber -= 1;
        }

        return maxNumber;
    }
}
