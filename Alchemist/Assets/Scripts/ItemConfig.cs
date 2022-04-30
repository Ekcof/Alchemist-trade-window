using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemConfig : MonoBehaviour
{
    [SerializeField] private string title;
    [SerializeField] private int itemNumber;
    [SerializeField] private int price;
    [SerializeField] private float weight;
    [SerializeField] private Sprite itemSprite;
    [SerializeField] private GameObject model;
    [SerializeField] private bool isPremium;
    
    /// <summary>
    /// Get the title of item
    /// </summary>
    /// <returns></returns>
    public string GetTitle()
    {
        return title;
    }

    public int GetNumber()
    {
        return itemNumber;
    }

    public int GetPrice()
    {
        return price;
    }

    public float GetWeight()
    {
        return weight;
    }

    public Sprite GetSprite()
    {
        return itemSprite;
    }

    public void SetNumber(int newItemNumber)
    {
        itemNumber = newItemNumber;
        if (itemNumber<newItemNumber) Debug.Log("Adding number!");
        if (itemNumber <= 0) Destroy(gameObject);
    }

    public void SetPrice(int newPrice)
    {
        price = newPrice;
    }

    public void SetWeight(float newWeight)
    {
        weight = newWeight;
    }

    public void DestroyItem()
    {
        Destroy(gameObject);
    }

    public bool GetIsPremium()
    {
        return isPremium;
    }
}
