using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    [SerializeField] public UnityEvent<RuntimeDialogueGraph> OnDialogue;
    [SerializeField] public UnityEvent<ItemToCount[]> OnAddItems, OnRemoveItems;

    void Awake()
    {
        if (Instance) return;
        Instance = this;
    }
}
