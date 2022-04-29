using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsScript : MonoBehaviour
{
    [SerializeField] private int money;
    private ItemObject itemObject;
    List<ItemObject> itemList = new List<ItemObject>();
    [SerializeField] private Sprite lemon;
    private void Awake()
    {
        itemObject = GetComponent<ItemObject>();
        //itemList.Add(new ItemObject("Lemon", 1, 10, 0.5f, lemon)) ;
    }
    public void RefreshItemList()
    {

    }
    
}
