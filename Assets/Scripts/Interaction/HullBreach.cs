using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HullBreach : InteractbleWithItem
{
    private List<Material> _outlines = new List<Material>();
    [SerializeField] private float _crashVolume = 1f;

    protected override void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactable");
        foreach(GameObject part in _parts)
        {
            Mesh mesh = part.GetComponent<MeshFilter>().mesh;
            mesh.subMeshCount+=1;
            mesh.SetTriangles(mesh.triangles,mesh.subMeshCount-1);
            MeshRenderer meshRenderer = part.GetComponent<MeshRenderer>();
            List<Material> materials = new(meshRenderer.materials);
            Material outline = null;
            foreach (Material m in materials)
            {
                if(m.name.IndexOf("InkingMaterial") >= 0) outline = m;
            }
            if (outline == null)
            {
                Material mat = Resources.Load<Material>("Materials/InkingMaterial");
                if(mat == null) Debug.LogWarning("material not found");
                else
                {
                    outline = new Material(mat);
                    materials.Add(outline);
                    meshRenderer.SetMaterials(materials);
                }
            }
            _outlines.Add(outline);
        }
    }

    protected override void Start()
    {
        SFXManager.PlaySound(SFXManager.SoundType.CRASH,_crashVolume);
    }

    public override void Interact(Player player)
    {
        // use the item

        EquippableItem item = player.PlayerInteract.EquippedItem;
        if(item != null && item.ItemName == _correctItem)
        {
            SFXManager.PlaySound(SFXManager.SoundType.ITEM_CLANG,item.ClangVolume);
            Destroy(gameObject); // destroy the breach after using the item
        }
    }

    protected override string UniqueToolTip(EquippableItem equippedItem)
    {
        return $"Repair Breach";
    }

    public override void highlight(Color color)
    {
        foreach(Material outline in _outlines)
        {
            if (_canInteract) outline?.SetColor("_Outline_Color",color);
            else outline?.SetColor("_Outline_Color",Color.black);
        }
    }
}
