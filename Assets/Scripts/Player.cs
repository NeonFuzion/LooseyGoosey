using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Inventory InventoryInstance;

    [SerializeField] float detectRadius;
    [SerializeField] LayerMask interactableLayers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (InventoryInstance) Destroy(InventoryInstance);
        InventoryInstance = GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleInteractInput()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectRadius, interactableLayers);

        if (colliders.Length == 0) return;
        foreach (Collider2D collider in colliders)
        {
            Interactable interactable = collider.GetComponent<Interactable>();

            if (!interactable.enabled) continue;
            interactable.InvokeOnInteract();
        }
    }
}
