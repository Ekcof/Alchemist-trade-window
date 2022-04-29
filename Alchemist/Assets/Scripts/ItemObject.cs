using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject
{
    private GameObject prefab;
    private string title;
    private int itemNumber;
    private int price;
    private float weight;
    private Sprite itemSprite;

    public ItemObject(string title, int itemNumber, int price, float weight, Sprite itemSprite)
    {
        this.title = title;
        this.itemNumber = itemNumber;
        this.price = price;
        this.weight = weight;
        this.itemSprite = itemSprite;
    }
    public string Title
    {
        get { return title; }
        set { title = value; }
    }
    public int ItemNumber
    {
        get { return itemNumber; }
        set { itemNumber = value; }
    }
    public int Price
    {
        get { return price; }
        set { price = value; }
    }
    public float Weight
    {
        get { return weight; }
        set { weight = value; }
    }
    public Sprite ItemSprite
    {
        get { return itemSprite; }
        set { itemSprite = value; }
    }
}
