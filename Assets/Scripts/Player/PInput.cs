using UnityEngine;
using UnityEngine.InputSystem;

public class PInput : PlayerSystem
{
    private PlayerInput _playerInput;

    protected override void Awake()
    {
        base.Awake();
        _playerInput = GetComponent<PlayerInput>(); // this creates unnecessary dependency with Interactable. Change this
    }

    void Start()
    {
        Interactable.setPI(_playerInput);
    }

    void Update()
    {
        player.LookDirection = player.LookInput.ReadValue<Vector2>();
        player.MoveDirection = player.MovementInput.ReadValue<Vector2>();
    }
}
