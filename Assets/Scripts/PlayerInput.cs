using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour, PlayerInputActions.IDialogueActions, PlayerInputActions.IGeneralActions, PlayerInputActions.IInventoryActions
{
    [Header("General")]
    [SerializeField] UnityEvent onDash;
    [SerializeField] UnityEvent onInteract;
    [SerializeField] UnityEvent onOpenInventory;
    [SerializeField] UnityEvent<Vector2> onMovement;

    [Header("Dialogue")]
    [SerializeField] UnityEvent onContinue;
    [SerializeField] UnityEvent onDecline;
    [SerializeField] UnityEvent<Vector2> onChangeSelection;

    [Header("Inventory")]
    [SerializeField] UnityEvent onCloseInventory;

    PlayerInputActions inputActions;

    void Awake()
    {
        inputActions = new PlayerInputActions();

        inputActions.General.SetCallbacks(this);
        inputActions.Dialogue.SetCallbacks(this);
        inputActions.Inventory.SetCallbacks(this);

        inputActions.General.Enable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartDialogue()
    {
        inputActions.Dialogue.Enable();
        inputActions.General.Disable();
    }

    public void OnEndDialogue()
    {
        inputActions.Dialogue.Disable();
        inputActions.General.Enable();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        onDash?.Invoke();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        onInteract?.Invoke();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        onMovement?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnNext(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        onContinue?.Invoke();
    }

    public void OnDecline(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        onDecline?.Invoke();
    }

    public void OnChangeSelection(InputAction.CallbackContext context)
    {
        onChangeSelection?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        inputActions.General.Disable();
        inputActions.Inventory.Enable();
        onOpenInventory?.Invoke();
    }

    public void OnClose(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        inputActions.General.Enable();
        inputActions.Inventory.Disable();
        onCloseInventory?.Invoke();
    }
}
