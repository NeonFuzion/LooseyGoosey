using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Inventory InventoryInstance;

    [SerializeField] float detectRadius;
    [SerializeField] LayerMask interactableLayers;

    Interactable interactable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (InventoryInstance) Destroy(InventoryInstance);
        InventoryInstance = GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectRadius, interactableLayers);

        if (colliders.Length == 0) return;
        Interactable closestInteractable = null;
        foreach (Collider2D collider in colliders)
        {
            if (closestInteractable && Vector2.Distance(transform.position, collider.transform.position) > 
                Vector2.Distance(transform.position, closestInteractable.transform.position))
                continue;
            if (collider.GetComponent<Interactable>() is Interactable newInteractable)
            {
                if (!newInteractable.enabled) continue;
                closestInteractable = newInteractable;
            }
        }

        if (interactable != closestInteractable)
        {
            interactable?.InvokeOnExit();
            closestInteractable?.InvokeOnEnter();
            interactable = closestInteractable;
        }
        if (interactable &&
            Vector2.Distance(interactable.transform.position, transform.position) > detectRadius)
        {
            interactable.InvokeOnExit();
            interactable = null;
        }
    }

    public void HandleInteractInput()
    {
        interactable?.InvokeOnInteract();
    }
}
