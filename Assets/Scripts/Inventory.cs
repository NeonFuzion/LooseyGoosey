using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] ItemToCount[] startItems;

    Dictionary<ItemSO, int> items;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        items = new Dictionary<ItemSO, int>();

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

    public int GetItemCount(ItemSO item)
    {
        if (!items.ContainsKey(item)) return 0;
        return items[item];
    }

    public ItemToCount[] GetItems()
    {
        ItemToCount[] output = new ItemToCount[items.Count];
        int index = 0;
        foreach (KeyValuePair<ItemSO, int> pair in items)
        {
            output[index++] = new ItemToCount()
            {
                Item = pair.Key,
                Count = pair.Value
            };
        }
        return output;
    }
}

[System.Serializable]
public class ItemToCount
{
    public ItemSO Item;
    public int Count;
}