using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerControllerScript : MonoBehaviour
{
    private PlayerInput _input;

    [SerializeField] private GameObject _pawn;
    [SerializeField] private GameEvent _onPauseTogglePressed;

    private IControllable _controllableScript;
    private CameraScript _cameraScript;
    private InteractScript _interactScript;

    void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _controllableScript = _pawn.GetComponent<IControllable>();
        _cameraScript = _pawn.GetComponent<CameraScript>();
        _interactScript = _pawn.GetComponent<InteractScript>();
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
        _input.actions["Pause"].performed += OnPauseToggle;
    }

    private void UnRegisterInGameControls()
    {
        _input.actions["Run"].performed -= RunStart;
        _input.actions["Run"].canceled -= RunEnd;
        _input.actions["Walk"].performed -= WalkStart;
        _input.actions["Walk"].canceled -= WalkEnd;
        _input.actions["Aim"].performed -= AimChanged;
        _input.actions["Pause"].performed -= OnPauseToggle;

        RunEnd(new());
        WalkEnd(new());
    }

    private void RegisterMenuControls()
    {
        _input.actions["Pause"].performed += OnPauseToggle;
    }

    private void UnRegisterMenuControls()
    {
        _input.actions["Pause"].performed += OnPauseToggle;
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

    private void OnPauseToggle(InputAction.CallbackContext context)
    {
        _onPauseTogglePressed?.Raise(this);
    }

}
