using UnityEngine;
using UnityEngine.InputSystem;

public class PData : MonoBehaviour
{
    // Input
    public PlayerInput PlayerInput { get; private set; }
    public InputAction MovementInput {get; private set;}
    public InputAction LookInput {get; private set;}
    public InputAction InteractInput {get; private set;}
    public InputAction DropInput { get; private set; }

    

    void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();
        MovementInput = PlayerInput.actions.FindAction("Move");
        LookInput = PlayerInput.actions.FindAction("Look");
        InteractInput = PlayerInput.actions.FindAction("Interact");
        DropInput = PlayerInput.actions.FindAction("Drop");
    }
}
