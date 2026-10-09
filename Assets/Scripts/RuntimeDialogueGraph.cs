using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class RuntimeDialogueGraph : ScriptableObject
{
    public string EntryNodeID;

    public List<RuntimeDirectNode> DialogueNodes = new List<RuntimeDirectNode>();
    public List<RuntimeChoiceNode> ChoiceNodes = new List<RuntimeChoiceNode>();
    public List<RuntimeQuestProgressNode> QuestProgressNodes = new List<RuntimeQuestProgressNode>();
    public List<RuntimeItemCheckNode> ItemCheckNodes = new List<RuntimeItemCheckNode>();
}

[System.Serializable]
public class RuntimeNode
{
    public string NodeID;
}

[System.Serializable]
public class RuntimeDialogueNode : RuntimeNode
{
    public Speaker Speaker;
    public string DialogueText;
}

[System.Serializable]
public class RuntimeDirectNode : RuntimeDialogueNode
{
    public string NextNodeID;
}

[System.Serializable]
public class RuntimeChoiceNode : RuntimeDialogueNode
{
    public List<ChoiceData> Choices = new ();
}

[System.Serializable]
public class ChoiceData
{
    public string ChoiceText, DestinationNodeID;
}

[System.Serializable]
public class RuntimeQuestProgressNode : RuntimeNode
{
    public string NextNodeID;
    public QuestSO Quest;
}

[System.Serializable]
public class RuntimeItemCheckNode : RuntimeNode
{
    public string SuccessNodeID, FailNodeID;
    public List<ItemToCount> Ingredients = new List<ItemToCount>();
}