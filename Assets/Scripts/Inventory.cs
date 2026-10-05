using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] ItemToCount[] startItems;

    Dictionary<ItemSO, int> items;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (ItemToCount itemPair in startItems)
        {
            AddItem(itemPair.Item, itemPair.Count);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(ItemSO item, int amount)
    {
        if (items.ContainsKey(item))
        {
            items[item] += amount;
        }
        else
        {
            items.Add(item, amount);
        }
    }

    public bool RemoveItem(ItemSO item, int amount)
    {
        if (!items.ContainsKey(item)) return false;
        if (items[item] - amount < 0) return false;
        else if (items[item] == 0) items.Remove(item);
        else items[item] -= amount;
        return true;
    }
}

[System.Serializable]
public class ItemToCount
{
    public ItemSO Item;
    public int Count;
}