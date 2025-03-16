using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    public PlayerInteract PlayerInteract {get; private set;}
    [SerializeField] private PlayerCamera _playerCamera;
    [SerializeField] private InputActionReference _movementInput,_lookInput,_interactInput;
    void OnEnable()
    {
        _interactInput.action.performed+=Use;
    }

    private void Use(InputAction.CallbackContext context) => PlayerInteract.Target?.Interact(this);

    void OnDisable()
    {
        _interactInput.action.performed-=Use;
    }

    void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        PlayerInteract = GetComponent<PlayerInteract>();
        PlayerInteract.PCam = _playerCamera;
    }

    void Update()
    {
        _playerCamera.LookDirection = _lookInput.action.ReadValue<Vector2>();
        _playerMovement.MoveDirection = _movementInput.action.ReadValue<Vector2>();
    }
}
