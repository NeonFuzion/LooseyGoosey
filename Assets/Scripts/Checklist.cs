using TMPro;
using UnityEngine;

public class Checklist : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] Transform ingredientParent;
    [SerializeField] GameObject checklistItemPrefab;

    Animator animator;

    int appearAnimationHash = Animator.StringToHash("ChecklistAppear");
    int disappearAnimationHash = Animator.StringToHash("ChecklistDisappear");

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateNewChecklist(Recipe recipe)
    {
        titleText.SetText(recipe.Item.Name);

        foreach (ItemToCount ingredient in recipe.Ingredients)
        {
            GameObject checklistItem = Instantiate(checklistItemPrefab, ingredientParent);
            checklistItem.GetComponentInChildren<TextMeshProUGUI>().SetText($"- {ingredient.Item} x{ingredient.Count}");
        }

        animator.CrossFade(appearAnimationHash, 0, 0);
    }

    public void HideChecklist()
    {
        animator.CrossFade(disappearAnimationHash, 0, 0);
    }
}
