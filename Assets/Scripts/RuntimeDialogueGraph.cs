using System.Collections.Generic;
using UnityEngine;

public class RuntimeDialogueGraph : ScriptableObject
{
    public string EntryNodeID;

    public List<RuntimeDialogueNode> DialogueNodes = new ();
    public List<RuntimeChoiceNode> ChoiceNodes = new ();
    public List<RuntimeQuestProgressNode> QuestProgressNodes = new ();
    public List<RuntimeItemCheckNode> ItemCheckNodes = new List<RuntimeItemCheckNode>();
}

[System.Serializable]
public class RuntimeNode
{
    public Speaker Speaker;
    public string NodeID, DialogueText;
}

[System.Serializable]
public class RuntimeDialogueNode : RuntimeNode
{
    public string NextNodeID;
}

[System.Serializable]
public class RuntimeChoiceNode : RuntimeNode
{
    public List<ChoiceData> Choices = new ();
}

[System.Serializable]
public class ChoiceData
{
    public string ChoiceText, DestinationNodeID;
}

[System.Serializable]
public class RuntimeQuestProgressNode
{
    public string NodeID, NextNodeID;
    public ItemSO Item;
}

[System.Serializable]
public class RuntimeItemCheckNode
{
    public string NodeID, SuccessNodeID, FailNodeID;
    public List<ItemToCount> Ingredients = new List<ItemToCount>();
}