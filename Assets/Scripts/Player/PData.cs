using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PData : MonoBehaviour
{
    [Header("Input")]
    public InputAction MovementInput {get; private set;}
    public InputAction LookInput {get; private set;}
    public InputAction InteractInput {get; private set;}
    public InputAction DropInput {get; private set; }

    [Header("Interaction")]
    public Interactable Target {get; set; }
    public HeavyItem HauledItem {get; set;}
    public EquippableItem EquippedItem {get; set;}
    public Action<EquippableItem> EquipEvent;
    public Action<HeavyItem> HaulEvent; // this might be kinda hacky. Change later?
    public Action DropHeavyEvent;

    [Header("Camera")]
    public Vector2 LookDirection {get; set;}

    [Header("Movement")]
    public Vector2 MoveDirection {get; set;}
    public float BaseSpeed {get; set;}
    public float CurrentSpeed {get; set;}

    void Awake()
    {
        PlayerInput playerInput = GetComponent<PlayerInput>();
        MovementInput = playerInput.actions.FindAction("Move");
        LookInput = playerInput.actions.FindAction("Look");
        InteractInput = playerInput.actions.FindAction("Interact");
        DropInput = playerInput.actions.FindAction("Drop");
    }
}
