using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] GameObject DialoguePanel, SpeakerPanel;
    [SerializeField] TextMeshProUGUI SpeakerNameText, DialogueText;
    [SerializeField] Image SpeakerSprite;

    [Header("Choice UI")]
    [SerializeField] GameObject ChoiceButtonPrefab;
    [SerializeField] Transform ChoiceButtonParent;

    [Header("Quest UI")]
    [SerializeField] Checklist checklist;

    [Header("Events")]
    [SerializeField] UnityEvent onDialogueStarted;
    [SerializeField] UnityEvent onDialogueEnded;

    private Dictionary<string, RuntimeNode> _nodeLookup = new ();
    private RuntimeNode _currentNode;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ProcessRuntimeNode(string nodeID)
    {
        if (!string.IsNullOrEmpty(nodeID) && _nodeLookup.ContainsKey(nodeID))
        {
            ShowNode(nodeID);
        }
        else
        {
            EndDialogue();
        }
    }

    void ShowNode(string nodeID)
    {
        _currentNode = _nodeLookup[nodeID];

        foreach (Transform child in ChoiceButtonParent)
        {
            Destroy(child.gameObject);
        }

        if (_currentNode is RuntimeDialogueNode dialogueNode) ShowDialogueNode(dialogueNode);
        switch (_currentNode)
        {
            case RuntimeChoiceNode choiceNode: ShowChoiceNode(choiceNode); break;
            case RuntimeQuestProgressNode questProgressNode: ShowQuestProgressNode(questProgressNode); break;
            case RuntimeItemCheckNode itemCheckNode: ShowItemCheckNode(itemCheckNode); break;
        }
    }

    void ShowDialogueNode(RuntimeDialogueNode dialogueNode)
    {
        DialogueText.SetText(dialogueNode.DialogueText);
        if (dialogueNode.Speaker is Speaker speaker)
        {
            SpeakerPanel.SetActive(true);
            SpeakerNameText.SetText(speaker.Name);
            SpeakerSprite.sprite = speaker.Sprite;
        }
        else
        {
            SpeakerPanel.SetActive(false);
        }
    }

    void ShowChoiceNode(RuntimeChoiceNode choiceNode)
    {
        foreach (ChoiceData choice in choiceNode.Choices)
        {
            GameObject buttonGameObject = Instantiate(ChoiceButtonPrefab, ChoiceButtonParent);

            Button button = buttonGameObject.GetComponent<Button>();
            button.onClick.AddListener(() => ProcessRuntimeNode(choice.DestinationNodeID));

            TextMeshProUGUI textMeshPro = button.GetComponentInChildren<TextMeshProUGUI>();
            textMeshPro.SetText(choice.ChoiceText);
        }
    }

    void ShowQuestProgressNode(RuntimeQuestProgressNode questProgressNode)
    {
        QuestSO quest = questProgressNode.Quest;
        if (checklist.IsVisible)
        {
            checklist.HideChecklist();
            ProcessRuntimeNode(questProgressNode.NextNodeID);
        }
        else
        {
            checklist.CreateNewChecklist(quest, QuestState.NotStarted);
        }
        QuestManager.Instance.UpdateQuest(quest);
    }

    void ShowItemCheckNode(RuntimeItemCheckNode itemCheckNode)
    {
        string nextNodeID = itemCheckNode.SuccessNodeID;
        foreach (ItemToCount item in itemCheckNode.Ingredients)
        {
            if (Player.InventoryInstance.GetItemCount(item.Item) >= item.Count) continue;
            nextNodeID = itemCheckNode.FailNodeID;
            break;
        }

        GameObject button = Instantiate(ChoiceButtonPrefab, ChoiceButtonParent);
        button.GetComponentInChildren<TextMeshProUGUI>().SetText("Submit");
        button.GetComponent<Button>().onClick.AddListener(() => ProcessRuntimeNode(nextNodeID));
    }

    void EndDialogue()
    {
        _currentNode = null;

        foreach (Transform child in ChoiceButtonParent)
        {
            Destroy(child.gameObject);
        }

        onDialogueEnded?.Invoke();
        DialoguePanel.SetActive(false);
    }

    public void StartDialogue(RuntimeDialogueGraph runtimeGraph)
    {
        foreach (RuntimeDirectNode node in runtimeGraph.DialogueNodes)
        {
            _nodeLookup[node.NodeID] = node;
        }
        foreach (RuntimeChoiceNode node in runtimeGraph.ChoiceNodes)
        {
            _nodeLookup[node.NodeID] = node;
        }
        foreach (RuntimeItemCheckNode node in runtimeGraph.ItemCheckNodes)
        {
            _nodeLookup[node.NodeID] = node;
        }
        foreach (RuntimeQuestProgressNode node in runtimeGraph.QuestProgressNodes)
        {
            _nodeLookup[node.NodeID] = node;
        }

        DialoguePanel.SetActive(true);
        onDialogueStarted?.Invoke();
        ProcessRuntimeNode(runtimeGraph.EntryNodeID);
    }

    public void HandleContinueDialogueInput()
    {
        switch (_currentNode)
        {
            case RuntimeDirectNode dialogueNode: ProcessRuntimeNode(dialogueNode.NextNodeID); break;
            case RuntimeQuestProgressNode progressNode: ShowQuestProgressNode(progressNode); break;
        }
    }

    public void HandleDeclineInput()
    {
        if (_currentNode is RuntimeChoiceNode choiceNode)
        {
            ProcessRuntimeNode(choiceNode.Choices.Last().DestinationNodeID);
        }
    }
}
