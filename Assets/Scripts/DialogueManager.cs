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

    void ProcessDialogueNode(string nodeID)
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

        switch (_currentNode)
        {
            case RuntimeDialogueNode dialogueNode: ShowDialogueNode(dialogueNode); break;
            case RuntimeChoiceNode choiceNode: ShowChoiceNode(choiceNode); break;
        }
    }

    void ShowDialogueNode(RuntimeDialogueNode dialogueNode)
    {
        foreach (Transform child in ChoiceButtonParent)
        {
            Destroy(child.gameObject);
        }

        DialogueText.SetText(_currentNode.DialogueText);
        if (_currentNode.Speaker is Speaker speaker)
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

    void ShowQuestProgressNode(RuntimeQuestProgressNode questProgressNode)
    {
        checklist.CreateNewChecklist(questProgressNode.Item);

    }

    void ShowChoiceNode(RuntimeChoiceNode choiceNode)
    {
        foreach (Transform child in ChoiceButtonParent)
        {
            Destroy(child.gameObject);
        }
        foreach (ChoiceData choice in choiceNode.Choices)
        {
            GameObject buttonGameObject = Instantiate(ChoiceButtonPrefab, ChoiceButtonParent);

            Button button = buttonGameObject.GetComponent<Button>();
            button.onClick.AddListener(() => ProcessDialogueNode(choice.DestinationNodeID));

            TextMeshProUGUI textMeshPro = button.GetComponentInChildren<TextMeshProUGUI>();
            textMeshPro.SetText(choice.ChoiceText);
        }
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
        foreach (RuntimeDialogueNode node in runtimeGraph.DialogueNodes)
        {
            _nodeLookup[node.NodeID] = node;
        }
        foreach (RuntimeChoiceNode node in runtimeGraph.ChoiceNodes)
        {
            _nodeLookup[node.NodeID] = node;
        }

        DialoguePanel.SetActive(true);
        onDialogueStarted?.Invoke();
        ProcessDialogueNode(runtimeGraph.EntryNodeID);
    }

    public void HandleContinueDialogueInput()
    {
        if (_currentNode is RuntimeDialogueNode dialogueNode)
        {
            ProcessDialogueNode(dialogueNode.NextNodeID);
        }
    }

    public void HandleDeclineInput()
    {
        if (_currentNode is RuntimeChoiceNode choiceNode)
        {
            ProcessDialogueNode(choiceNode.Choices.Last().DestinationNodeID);
        }
    }
}
