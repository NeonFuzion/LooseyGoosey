using UnityEngine;

public class Recipe : ScriptableObject
{
    [field: SerializeField] public ItemSO Item { get; private set; }
    [field: SerializeField] public ItemToCount[] Ingredients { get; private set; }
}
