using System.Collections.Generic;
using UnityEngine;

// TODO this does nothing right now.
/// <summary>
/// A suit that gives the player oxygen
/// </summary>
public class Suit : EventInteractable
{
    [SerializeField] private MeshRenderer helmetMeshRenderer;
    private Material helmetOutline;

    protected override void Awake()
    {
        base.Awake();
        // we need to also highlight the mesh of the helmet
        if (helmetMeshRenderer == null)
        {
            Debug.LogWarning("Helmet Mesh Render not assigned.");
            return;
        }

        List<Material> materials = new(helmetMeshRenderer.materials);
        foreach (Material m in materials)
        {
            if (m.name.IndexOf("InkingMaterial") >= 0) helmetOutline = m;
        }
        if (helmetOutline == null)
        {
            Material mat = Resources.Load<Material>("Materials/InkingMaterial");
            if (mat == null) Debug.LogWarning("material not found");
            else
            {
                helmetOutline = new Material(mat);
                materials.Add(helmetOutline);
                helmetMeshRenderer.SetMaterials(materials);
            }
        }
    }

    public override void Interact(PData player)
    {
        SFXManager.PlaySound(SFXManager.SoundType.SUIT_EQUIP);
        Destroy(gameObject);
    }

    protected override string UniqueToolTip(EquippableItem equippedItem)
    {
        return $"Put on Suit";
    }

    public override void Highlight(Color color)
    {
        base.Highlight(color);
        helmetOutline?.SetColor("_Outline_Color", color);
    }
}
