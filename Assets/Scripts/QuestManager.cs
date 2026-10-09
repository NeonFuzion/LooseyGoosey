using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    Dictionary<QuestSO, QuestState> questStates;

    void Awake()
    {
        if (Instance) return;
        Instance = this;

        questStates = new Dictionary<QuestSO, QuestState>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateQuest(QuestSO quest)
    {
        if (!questStates.ContainsKey(quest)) questStates.Add(quest, QuestState.None);
        if (questStates[quest] == QuestState.Complete) return;
        questStates[quest]++;
    }

    public QuestState GetQuestState(QuestSO quest)
    {
        if (!questStates.ContainsKey(quest)) return QuestState.None;
        return questStates[quest];
    }
}
