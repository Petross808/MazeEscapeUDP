using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerControllerScript : MonoBehaviour
{
    private PlayerInput _input;

    [SerializeField] private GameObject _pawn;
    [SerializeField, EventSignature] private GameEvent _onPauseTogglePressed;
    [SerializeField, EventSignature] private GameEvent _onEscapeMenuPressed;

    private IControllable _controllableScript;
    private CameraScript _cameraScript;
    private InteractScript _interactScript;
    private PlayerInventory _playerInventory;

    void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _controllableScript = _pawn.GetComponent<IControllable>();
        _cameraScript = _pawn.GetComponent<CameraScript>();
        _interactScript = _pawn.GetComponent<InteractScript>();
        _playerInventory = _pawn.GetComponent<PlayerInventory>();
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        UnRegisterInGameControls();
        _input.SwitchCurrentActionMap("Menu");
        RegisterMenuControls();
    }

    private void OnDisable()
    {
        UnRegisterInGameControls();
        UnRegisterMenuControls();
    }

    private void RegisterInGameControls()
    {
        _input.actions["Run"].performed += RunStart;
        _input.actions["Run"].canceled += RunEnd;
        _input.actions["Walk"].performed += WalkStart;
        _input.actions["Walk"].canceled += WalkEnd;
        _input.actions["Aim"].performed += AimChanged;
        _input.actions["Interact"].performed += OnInteract;
        _input.actions["Drop"].performed += OnDrop;
        _input.actions["Pause"].performed += OnPauseToggle;
        _input.actions["Inspect"].performed += OnInspectStart;
        _input.actions["Inspect"].canceled += OnInspectEnd;
    }

    private void UnRegisterInGameControls()
    {
        _input.actions["Run"].performed -= RunStart;
        _input.actions["Run"].canceled -= RunEnd;
        _input.actions["Walk"].performed -= WalkStart;
        _input.actions["Walk"].canceled -= WalkEnd;
        _input.actions["Aim"].performed -= AimChanged;
        _input.actions["Interact"].performed -= OnInteract;
        _input.actions["Drop"].performed -= OnDrop;
        _input.actions["Pause"].performed -= OnPauseToggle;
        _input.actions["Inspect"].performed -= OnInspectStart;
        _input.actions["Inspect"].canceled -= OnInspectEnd;

        RunEnd(new());
        WalkEnd(new());
        OnInspectEnd(new());
    }

    private void RegisterMenuControls()
    {
        _input.actions["Pause"].performed += OnEscapeMenuPressed;
    }

    private void UnRegisterMenuControls()
    {
        _input.actions["Pause"].performed += OnEscapeMenuPressed;
    }

    [EventSignature(typeof(bool))]
    public void ChangeControls(GameEvent.CallbackContext context)
    {
        bool isPaused = context.Get<bool>();
        if(isPaused)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            UnRegisterInGameControls();
            _input.SwitchCurrentActionMap("Menu");
            RegisterMenuControls();
            
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            UnRegisterMenuControls();
            _input.SwitchCurrentActionMap("InGame");
            RegisterInGameControls();
        }
    }

    [EventSignature]
    public void BlockInput(GameEvent.CallbackContext _)
    {
        _input.DeactivateInput();
    }

    [EventSignature]
    public void UnblockInput(GameEvent.CallbackContext _)
    {
        _input.ActivateInput();
    }

    private void RunStart(InputAction.CallbackContext context)
    {
        _controllableScript?.SetMoveDirection(context.ReadValue<Vector2>());
    }

    private void RunEnd(InputAction.CallbackContext context)
    {
        _controllableScript?.SetMoveDirection(Vector2.zero);
    }

    private void WalkStart(InputAction.CallbackContext context)
    {
        _controllableScript?.SetWalking(true);
    }

    private void WalkEnd(InputAction.CallbackContext context)
    {
        _controllableScript?.SetWalking(false);
    }

    private void AimChanged(InputAction.CallbackContext context)
    {
        _cameraScript?.AimChanged(context.ReadValue<Vector2>());
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        _interactScript?.OnInteract();
    }

    private void OnDrop(InputAction.CallbackContext context)
    {
        _playerInventory?.DropItem();
    }

    private void OnPauseToggle(InputAction.CallbackContext context)
    {
        _onPauseTogglePressed.Raise(this);
    }

    private void OnEscapeMenuPressed(InputAction.CallbackContext context)
    {
        _onEscapeMenuPressed.Raise(this);
    }

    private void OnInspectStart(InputAction.CallbackContext context)
    {
        _interactScript?.OnInspect();
    }

    private void OnInspectEnd(InputAction.CallbackContext context)
    {
        _interactScript?.OnInspectStop();
    }

}
