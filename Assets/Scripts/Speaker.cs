using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeakerData")]
public class Speaker : ScriptableObject
{
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
}
