using System.Collections.Generic;
using UnityEngine;

public class Suit : EventInteractable
{
    [SerializeField] private MeshRenderer helmetMeshRenderer;
    private Material helmetOutline;

    protected override void Awake()
    {
        base.Awake();
        if (helmetMeshRenderer==null)
        {
            Debug.LogWarning("Helmet Mesh Render not assigned.");
            return;
        }

        List<Material> materials = new(helmetMeshRenderer.materials);
        foreach (Material m in materials)
        {
            if(m.name.IndexOf("InkingMaterial") >= 0) helmetOutline = m;
        }
        if (helmetOutline == null)
        {
            Material mat = Resources.Load<Material>("Materials/InkingMaterial");
            if(mat == null) Debug.LogWarning("material not found");
            else
            {
                helmetOutline = new Material(mat);
                materials.Add(helmetOutline);
                helmetMeshRenderer.SetMaterials(materials);
            }
        }
    }

    public override void Interact(Player player)
    {
        SFXManager.PlaySound(SFXManager.SoundType.SUIT_EQUIP);
        Destroy(gameObject);
    }

    protected override string UniqueToolTip(EquippableItem equippedItem)
    {
        return $"Put on Suit";
    }

    public override void highlight(Color color)
    {
        base.highlight(color);
        helmetOutline?.SetColor("_Outline_Color",color);
    }
}
