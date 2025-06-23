using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Data class which holds anything that multiple scripts share.
/// </summary>
public class PData : MonoBehaviour
{
    [Header("Input")]
    public InputAction MovementInput;
    public InputAction LookInput;
    public InputAction InteractInput;
    public InputAction DropInput;
    [SerializeField] private float _lookSensitivity;
    public float LookSensitivity { get => _lookSensitivity; }

    [Header("Interaction")]
    public Interactable Target { get; set; }
    public HeavyItem HauledItem { get; set; }
    public EquippableItem EquippedItem { get; set; }
    public Action<EquippableItem> EquipEvent;
    public Action<HeavyItem> HaulEvent; // this might be kinda hacky. Change later?
    public Action DropHeavyEvent;

    [Header("Camera")]
    public Vector2 LookDirection { get; set; }
    [SerializeField] private Transform _playerOrientation;
    public Transform PlayerOrientation { get => _playerOrientation; }

    [Header("Movement")]
    public Vector2 MoveDirection { get; set; }
    public float BaseSpeed { get; set; }
    public float CurrentSpeed { get; set; }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PData))]
public class PDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PData pd = (PData) target;
        if(GUILayout.Button("Show Interact Input",GUILayout.Width(90f)))
        {
            Debug.Log(pd.InteractInput);
        }
    }
}
#endif
