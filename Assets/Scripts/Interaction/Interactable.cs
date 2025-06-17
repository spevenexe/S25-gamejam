using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Object that can be interacted with by the player.
/// </summary>
public abstract class Interactable : MonoBehaviour
{
    [SerializeField] protected bool _canInteract = true;
    [SerializeField] protected GameObject[] _parts;

    protected Material _outline;
    protected static PlayerInput PInput;
    protected virtual void OnEnable() { }
    protected virtual void OnDisable() { }

    /// <summary>
    /// Attempt an interact with this object.
    /// </summary>
    /// <param name="player">The player data of the interacter.</param>
    public abstract void Interact(PData player);
    protected virtual void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactable");

        // this creates an additional mesh out of submeshes so we can fully highlight it
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.subMeshCount += 1;
        mesh.SetTriangles(mesh.triangles, mesh.subMeshCount - 1);
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        List<Material> materials = new(meshRenderer.materials); // this allocates new memory and can be extremely expensive. Switch to a global shader.
        // try and find the outline material
        foreach (Material m in materials)
        {
            if (m.name.IndexOf("InkingMaterial") >= 0) _outline = m;
        }
        // if we can't find it, load it
        if (_outline == null)
        {
            Material mat = Resources.Load<Material>("Materials/InkingMaterial");
            if (mat == null) Debug.LogWarning("material not found");
            else
            {
                _outline = new Material(mat);
                materials.Add(_outline);
                meshRenderer.SetMaterials(materials);
            }
        }
    }

    protected virtual void Start() { }

    /// <summary>
    /// Returns the message tooltip to display when hovering over this <c>Interactable</c>, plus the necessary keybind.
    /// </summary>
    /// <param name="equippedItem">The item equipped in the player's hand. It may alter the tooltip.</param>
    /// <returns>The string message tooltip for the object, plus the "interact" keybind.</returns>
    public virtual String MessageTooltip(EquippableItem equippedItem = null)
    {
        if (!_canInteract)
            return "";
        else
            return $"{Utils.getKeys(PInput, "Interact")} {UniqueToolTip(equippedItem)}";
    }

    /// <summary>
    /// Returns prompt text that describes what interaction will occur.
    /// </summary>
    /// <param name="equippedItem">The item equipped in the player's hand. It may alter the tooltip.</param>
    /// <returns>The tooltip string for the item.</returns>
    protected abstract String UniqueToolTip(EquippableItem equippedItem);

    /// <summary>
    /// Sets the static player input object. This is used for showing keybind tooltips. 
    /// </summary>
    /// <param name="pi">The <c>PlayerInput</c> object to reference.</param>
    public static void SetPI(PlayerInput pi)
    {
        PInput = pi;
    }

    /// <summary>
    /// Sets the outline color of interactable objects. 
    /// </summary>
    /// <param name="color">The specified highlight color.</param>
    public virtual void Highlight(Color color)
    {
        if (_canInteract) _outline?.SetColor("_Outline_Color", color);
        else _outline?.SetColor("_Outline_Color", Color.black);
    }
}
