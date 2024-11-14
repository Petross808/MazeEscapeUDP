using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerControllerScript : MonoBehaviour
{
    private PlayerInput _input;

    [SerializeField] private GameObject _pawn;

    private IControllable _controllableScript;
    private CameraScript _cameraScript;
    private InteractScript _interactScript;

    // Start is called before the first frame update
    void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _controllableScript = _pawn.GetComponent<IControllable>();
        _cameraScript = _pawn.GetComponent<CameraScript>();
        _interactScript = _pawn.GetComponent<InteractScript>();
    }

    private void OnEnable()
    {
        RegisterInGameControls();
    }

    private void OnDisable()
    {
        UnRegisterInGameControls();
    }

    private void RegisterInGameControls()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _input.actions["Run"].performed += RunStart;
        _input.actions["Run"].canceled += RunEnd;
        _input.actions["Walk"].performed += WalkStart;
        _input.actions["Walk"].canceled += WalkEnd;
        _input.actions["Aim"].performed += AimChanged;
        _input.actions["Interact"].performed += OnInteract;
    }

    private void UnRegisterInGameControls()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        _input.actions["Run"].performed -= RunStart;
        _input.actions["Run"].canceled -= RunEnd;
        _input.actions["Walk"].performed -= WalkStart;
        _input.actions["Walk"].canceled -= WalkEnd;
        _input.actions["Aim"].performed -= AimChanged;
        _input.actions["Interact"].performed -= OnInteract;
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
}
