using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    [SerializeField] private PlayerCamera _playerCamera;
    [SerializeField] private InputActionReference _movementInput,_lookInput;
    void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        _playerCamera.LookDirection = _lookInput.action.ReadValue<Vector2>();
        _playerMovement.MoveDirection = _movementInput.action.ReadValue<Vector2>();
    }
}
