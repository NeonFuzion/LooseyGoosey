using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] GameObject interactVisual;
    [SerializeField] UnityEvent onInteract;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InvokeOnInteract()
    {
        onInteract?.Invoke();
    }

    public void InvokeOnEnter()
    {
        interactVisual.SetActive(true);
    }

    public void InvokeOnExit()
    {
        interactVisual.SetActive(false);
    }
}
