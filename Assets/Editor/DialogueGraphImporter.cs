using UnityEngine;
using UnityEditor.AssetImporters;
using Unity.GraphToolkit.Editor;
using System.Collections.Generic;
using System.Linq;
using System;

[ScriptedImporter(1, DialogueGraph.AssetExtension)]
public class DialogueGraphImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        DialogueGraph editorGraph = GraphDatabase.LoadGraphForImporter<DialogueGraph>(ctx.assetPath);
        RuntimeDialogueGraph runtimeGraph = ScriptableObject.CreateInstance<RuntimeDialogueGraph>();
        Dictionary<INode, string> nodeIDMap = new ();

        foreach (INode node in editorGraph.GetNodes())
        {
            nodeIDMap[node] = System.Guid.NewGuid().ToString();
        }

        StartNode startNode = editorGraph.GetNodes().OfType<StartNode>().FirstOrDefault();
        if (startNode != null)
        {
            IPort entryPort = startNode.GetOutputPorts().FirstOrDefault()?.FirstConnectedPort;
            if (entryPort != null)
            {
                runtimeGraph.EntryNodeID = nodeIDMap[entryPort.GetNode()];
            }
        }

        foreach (INode iNode in editorGraph.GetNodes())
        {
            if (iNode is StartNode || iNode is EndNode) continue;
            string nodeID = nodeIDMap[iNode];
            switch (iNode)
            {
                case DialogueNode dialogueNode:
                    ProcessDialogueNode(dialogueNode, nodeID, nodeIDMap, runtimeGraph);
                    break;
                case ChoiceNode choiceNode:
                    ProcessChoiceNode(choiceNode, nodeID, nodeIDMap, runtimeGraph);
                    break;
                case QuestProgressNode questProgressNode:
                    ProcessQuestProgressNode(questProgressNode, nodeID, nodeIDMap, runtimeGraph);
                    break;
                case ItemCheckNode itemCheckNode:
                    ProcessItemCheckNode(itemCheckNode, nodeID, nodeIDMap, runtimeGraph);
                    break;
            }
        }

        ctx.AddObjectToAsset("RuntimeData", runtimeGraph);
        ctx.SetMainObject(runtimeGraph);
    }

    private void ProcessQuestProgressNode(QuestProgressNode node, string nodeID, Dictionary<INode, string> nodeIDMap, RuntimeDialogueGraph runtimeGraph)
    {
        RuntimeQuestProgressNode runtimeNode = new () { NodeID = nodeID };
        runtimeGraph.QuestProgressNodes.Add(runtimeNode);

        runtimeNode.Quest = GetPortValue<QuestSO>(node.GetInputPortByName("Quest"));

        if (node.GetOutputPortByName("out").FirstConnectedPort is IPort nextNodePort)
            runtimeNode.NextNodeID = nodeIDMap[nextNodePort.GetNode()];
    }

    private void ProcessDialogueNode(DialogueNode node, string nodeID, Dictionary<INode, string> nodeIDMap, RuntimeDialogueGraph runtimeGraph)
    {
        RuntimeDirectNode runtimeNode = new () { NodeID = nodeID };
        runtimeGraph.DialogueNodes.Add(runtimeNode);

        runtimeNode.Speaker = GetPortValue<Speaker>(node.GetInputPortByName("Speaker"));
        runtimeNode.DialogueText = GetPortValue<string>(node.GetInputPortByName("Dialogue"));

        if (node.GetOutputPortByName("out").FirstConnectedPort is IPort nextNodePort)
            runtimeNode.NextNodeID = nodeIDMap[nextNodePort.GetNode()];
    }

    private void ProcessChoiceNode(ChoiceNode node, string nodeID, Dictionary<INode, string> nodeIDMap, RuntimeDialogueGraph runtimeGraph)
    {
        RuntimeChoiceNode runtimeNode = new () { NodeID = nodeID };
        runtimeGraph.ChoiceNodes.Add(runtimeNode);

        runtimeNode.Speaker = GetPortValue<Speaker>(node.GetInputPortByName("Speaker"));
        runtimeNode.DialogueText = GetPortValue<string>(node.GetInputPortByName("Dialogue"));

        foreach (IPort outputPort in node.GetOutputPorts())
        {
            if (!outputPort.Name.StartsWith("out")) continue;
            string index = outputPort.Name.Split(" ")[1];
            IPort textPort = node.GetInputPortByName($"Choice Text {index}");

            ChoiceData choiceData = new ()
            {
                ChoiceText = GetPortValue<string>(textPort),
                DestinationNodeID = outputPort.FirstConnectedPort != null ? nodeIDMap[outputPort.FirstConnectedPort.GetNode()] : null
            };

            runtimeNode.Choices.Add(choiceData);
        }
    }

    private void ProcessItemCheckNode(ItemCheckNode node, string nodeID, Dictionary<INode, string> nodeIDMap, RuntimeDialogueGraph runtimeGraph)
    {
        RuntimeItemCheckNode runtimeNode = new () { NodeID = nodeID };
        runtimeGraph.ItemCheckNodes.Add(runtimeNode);
        

        foreach (IPort inputPorts in node.GetInputPorts())
        {
            string[] nameParts = inputPorts.Name.Split(" ");
            ItemToCount itemPair = new ItemToCount();
            switch (nameParts[0])
            {
                case "Ingredient": 
                    string ingredientsIndex = nameParts[1];
                    IPort ingredientTextPort = node.GetInputPortByName($"Ingredient {ingredientsIndex}");
                    itemPair.Item = GetPortValue<ItemSO>(ingredientTextPort);
                    break;
                case "Amount":
                    string amountIndex = nameParts[1];
                    IPort amountTextPort = node.GetInputPortByName($"Amount {amountIndex}");
                    itemPair.Count = GetPortValue<int>(amountTextPort);
                    
                    runtimeNode.Ingredients.Add(itemPair);
                    itemPair = null;
                    break;
            }
        }

        if (node.GetOutputPortByName("out Success").FirstConnectedPort is IPort successNodePort)
            runtimeNode.SuccessNodeID = nodeIDMap[successNodePort.GetNode()];
        if (node.GetOutputPortByName("out Fail").FirstConnectedPort is IPort failNodePort)
            runtimeNode.FailNodeID = nodeIDMap[failNodePort.GetNode()];
    }

    private T GetPortValue<T>(IPort port)
    {
        if (port == null) return default;

        if (port.IsConnected)
        {
            if (port.FirstConnectedPort.GetNode() is IVariableNode variableNode)
            {
                variableNode.Variable.TryGetDefaultValue(out T value);
                return value;
            }
        }

        port.TryGetValue(out T fallballValue);
        return fallballValue;
    }
}
