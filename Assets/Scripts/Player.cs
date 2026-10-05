using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float detectRadius;
    [SerializeField] LayerMask interactableLayers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleInteractInput()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, detectRadius, interactableLayers);

        if (!collider) return;
        collider.GetComponent<Interactable>().InvokeOnInteract();
    }
}
