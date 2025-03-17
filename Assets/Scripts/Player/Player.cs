using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    public PlayerInteract PlayerInteract {get; private set;}
    public PlayerCamera _playerCamera;
    [SerializeField] private InputActionReference _movementInput,_lookInput,_interactInput,_dropInput;
    void OnEnable()
    {
        _interactInput.action.performed+=Use;
        _dropInput.action.performed+=Drop;
    }

    private void Drop(InputAction.CallbackContext context) => PlayerInteract.Drop();

    // private void Use(InputAction.CallbackContext context) => PlayerInteract.Target?.Interact(this);
    private void Use(InputAction.CallbackContext context) 
    {
        Debug.Log(PlayerInteract.Target);
        PlayerInteract.Target?.Interact(this);
    }

    void OnDisable()
    {
        _interactInput.action.performed-=Use;
        _dropInput.action.performed-=Drop;
    }

    void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        PlayerInteract = GetComponent<PlayerInteract>();
    }

    void Update()
    {
        _playerCamera.LookDirection = _lookInput.action.ReadValue<Vector2>();
        _playerMovement.MoveDirection = _movementInput.action.ReadValue<Vector2>();
    }
}
