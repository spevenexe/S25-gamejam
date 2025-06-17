using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Script that handles all input reception. It stores the information in a corresponding PData object. 
/// </summary>
public class PInput : PlayerSystem
{
    private PlayerInput _playerInput;

    protected override void Awake()
    {
        base.Awake();
        _playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        _player.LookDirection = _player.LookInput.ReadValue<Vector2>();
        _player.MoveDirection = _player.MovementInput.ReadValue<Vector2>();
    }
}
