using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] bool hasQuest;
    [SerializeField] RuntimeDialogueGraph idleDialogueGraph;
    [SerializeField] QuestDialogue questDialogue;
    [SerializeField] QuestSO[] questData;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (hasQuest) QuestManager.Instance.UpdateQuest(questData[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInteract()
    {
        RuntimeDialogueGraph dialogueGraph = QuestManager.Instance.GetQuestState(questData[0]) switch
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