using UnityEngine;

[CreateAssetMenu(menuName = "Quest")]
public class QuestSO : ScriptableObject
{
    public string Name;
    public Speaker QuestGiver;
    public QuestTask[] Tasks;
}

public enum QuestState { None, NotStarted, InProgress, Complete }

[System.Serializable]
public class QuestTask
{
    public Speaker Target;
    public ItemToCount[] Goods;
}