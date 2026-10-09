using System;
using TMPro;
using UnityEngine;

public class Checklist : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] Transform ingredientParent;
    [SerializeField] GameObject checklistItemPrefab;

    public bool IsVisible { get; private set; }

    Animator animator;

    int appearAnimationHash = Animator.StringToHash("ChecklistAppear");
    int disappearAnimationHash = Animator.StringToHash("ChecklistDisappear");

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateNewChecklist(QuestSO quest, QuestState questState)
    {
        titleText.SetText(quest.Name);

        foreach (Transform child in ingredientParent)
        {
            Destroy(child.gameObject);
        }
        foreach (ItemToCount ingredient in quest.Tasks[0].Goods)
        {
            GameObject checklistItem = Instantiate(checklistItemPrefab, ingredientParent);
            string checklistItemText = questState switch
            {
                QuestState.InProgress => $"- {Player.InventoryInstance.GetItemCount(ingredient.Item)}/{ingredient.Count} {ingredient.Item} ",
                _ => $"- {ingredient.Count} {ingredient.Item.Name}"
            };
            checklistItem.GetComponent<TextMeshProUGUI>().SetText(checklistItemText);
        }

        IsVisible = true;
        animator.CrossFade(appearAnimationHash, 0, 0);
    }

    public void HideChecklist()
    {
        IsVisible = false;
        animator.CrossFade(disappearAnimationHash, 0, 0);
    }
}
