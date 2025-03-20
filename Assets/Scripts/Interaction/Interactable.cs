using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MeshRenderer),typeof(MeshFilter))]
public abstract class Interactable : MonoBehaviour
{
    [SerializeField] protected bool _canInteract = true;

    protected Material _outline;
    protected static PlayerInput PInput; 
    protected virtual void OnEnable(){}
    protected virtual void OnDisable(){}

    public abstract void Interact(Player player);
    protected virtual void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactable");

        // this creates an additional mesh out of submeshes so we can fully highlight it
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.subMeshCount+=1;
        mesh.SetTriangles(mesh.triangles,mesh.subMeshCount-1);
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        List<Material> materials = new(meshRenderer.materials);
        // try and find the outline material
        foreach (Material m in materials)
        {
            if(m.name.IndexOf("InkingMaterial") >= 0) _outline = m;
        }
        // if we can't find it, load it
        if (_outline == null)
        {
            Material mat = Resources.Load<Material>("Materials/InkingMaterial");
            if(mat == null) Debug.LogWarning("material not found");
            else
            {
                _outline = new Material(mat);
                materials.Add(_outline);
                meshRenderer.SetMaterials(materials);
            }
        }
    }

    protected virtual void Start(){}

    // return just the keys
    public String MessageTooltip()
    {
        if(!_canInteract)
            return "";
        else
            return $"{Utils.getKeys(PInput,"Interact")} {UniqueToolTip()}";
    }

    protected abstract String UniqueToolTip();

    public static void setPI(PlayerInput pi)
    {
        PInput = pi;
    } 

    public virtual void highlight(Color color)
    {
        if (_canInteract) _outline?.SetColor("_Outline_Color",color);
        else _outline?.SetColor("_Outline_Color",Color.black);
    }
}
