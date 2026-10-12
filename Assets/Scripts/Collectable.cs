using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] ItemSO item;

    bool isHarvestable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isHarvestable = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HarvestItem()
    {
        if (!isHarvestable) return;
        isHarvestable = false;
        Player.InventoryInstance.AddItem(item, 1);
    }
}
