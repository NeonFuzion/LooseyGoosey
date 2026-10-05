using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Item")]
public class ItemSO : ScriptableObject
{
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public String Name { get; private set; }
}