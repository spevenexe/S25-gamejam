using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    public PlayerInteract PlayerInteract {get; private set;}
    public PlayerCamera _playerCamera;
    private PlayerInput _playerInput;
    private InputAction _movementInput,_lookInput,_interactInput,_dropInput;
    void OnEnable()
    {
        _playerInput = GetComponent<PlayerInput>();
        _movementInput = _playerInput.actions.FindAction("Move");
        _lookInput = _playerInput.actions.FindAction("Look");
        _interactInput = _playerInput.actions.FindAction("Interact");
        _dropInput = _playerInput.actions.FindAction("Drop");
        _interactInput.performed+=Use;
        _dropInput.performed+=Drop;
    }

    private void Drop(InputAction.CallbackContext context) => PlayerInteract.Drop();

    private void Use(InputAction.CallbackContext context)
    {
        if(PlayerInteract.Target == null) SFXManager.PlaySound(SFXManager.SoundType.INTERACT_FAIL,0.3f);
        else PlayerInteract.Target?.Interact(this);
    }

    void OnDisable()
    {
        _interactInput.performed-=Use;
        _dropInput.performed-=Drop;
    }

    void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        PlayerInteract = GetComponent<PlayerInteract>();
    }

    void Start()
    {
        Interactable.setPI(_playerInput);
    }

    void Update()
    {
        _playerCamera.LookDirection = _lookInput.ReadValue<Vector2>();
        _playerMovement.MoveDirection = _movementInput.ReadValue<Vector2>();
    }
}
