using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshRenderer),typeof(MeshFilter))]
public abstract class Interactable : MonoBehaviour
{
    protected bool canInteract = true;

    protected Material outline;
    protected static PlayerInput PInput; 
    protected virtual void OnEnable(){}
    protected virtual void OnDisable(){}

    public abstract void Interact(Player player);
    protected virtual void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactable");

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.subMeshCount+=1;
        mesh.SetTriangles(mesh.triangles,mesh.subMeshCount-1);
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        List<Material> materials = new(meshRenderer.materials);
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
    }

    protected virtual void Start(){}

    // return just the keys
    public virtual String MessageTooltip()
    {
        return Utils.getKeys(PInput,"Interact");
    }
    public static void setPI(PlayerInput pi)
    {
        PInput = pi;
    } 

    public virtual void highlight(Color color)
    {
        if (canInteract) outline?.SetColor("_Outline_Color",color);
        else outline?.SetColor("_Outline_Color",Color.black);
    }
}
