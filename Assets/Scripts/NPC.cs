using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] RuntimeDialogueGraph idleDialogueGraph;
    [SerializeField] QuestDialogue questDialogue;
    [SerializeField] QuestData questData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        questData.CurrentState = QuestState.NotStarted;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInteract()
    {
        RuntimeDialogueGraph dialogueGraph = questData.CurrentState switch
        {
            QuestState.NotStarted => questDialogue.StartQuestDialogue,
            QuestState.InProgress => questDialogue.InProgressQuestDialogue,
            _ => idleDialogueGraph
        };

        EventManager.Instance.OnDialogue?.Invoke(dialogueGraph);
    }
}

[System.Serializable]
public struct QuestDialogue
{
    [field: SerializeField] public RuntimeDialogueGraph StartQuestDialogue { get; private set; }
    [field: SerializeField] public RuntimeDialogueGraph InProgressQuestDialogue { get; private set; }
}

[CreateAssetMenu(menuName = "Quest")]
public class QuestData : ScriptableObject
{
    public string Name;
    public QuestState CurrentState;
    public Recipe Recipe;
}

public enum QuestState { None, NotStarted, InProgress, Complete }