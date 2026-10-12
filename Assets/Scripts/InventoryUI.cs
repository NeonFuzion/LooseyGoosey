using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject prefabItemSlot;
    [SerializeField] Transform itemParent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show()
    {
        ItemToCount[] items = Player.InventoryInstance.GetItems();
        int i = 0;
        for (; i < items.Length; i++)
        {
            Transform itemSlot = null;
            if (i < itemParent.childCount)
            { 
                itemSlot = itemParent.GetChild(i);
                itemSlot.gameObject.SetActive(true);
            }
            else
            {
                itemSlot = Instantiate(prefabItemSlot, itemParent).transform;
            }
            itemSlot.GetChild(0).GetComponent<Image>().sprite = items[i].Item.Sprite;
            itemSlot.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(items[i].Count + "");
        }
        for (; i < itemParent.childCount; i++)
        {
            itemParent.GetChild(i).gameObject.SetActive(false);
        }
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
