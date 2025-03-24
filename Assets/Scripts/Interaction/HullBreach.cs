using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// TODO: This is Hella slow. Fix Awake()
public class HullBreach : InteractbleWithItem
{
    private List<Material> _outlines = new List<Material>();
    [SerializeField] private float _crashVolume = 1f;
    
    [SerializeField] private float _growMinTime = 10f;
    [SerializeField] private float _growMaxTime = 15f;
    private float _timer = 0 ;
    public bool Infested {get;private set;} = false;
    private static HullBreach _infestedHullBreach;

    public static bool BreachDestroyedOnce = false; // for tutorial

    protected override void Awake()
    {
        if(_infestedHullBreach == null)
        {
            _infestedHullBreach = Resources.Load<HullBreach>("Interactables/Infested Hull Breach");
        }

        gameObject.layer = LayerMask.NameToLayer("Interactable");
        
        // for meshes located on the object itself
        MeshFilter mf;
        if(TryGetComponent(out mf))
        {
            Mesh mesh = GetComponent<MeshFilter>().mesh;
            mesh.subMeshCount+=1;
            mesh.SetTriangles(mesh.triangles,mesh.subMeshCount-1);
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            List<Material> materials = new(meshRenderer.materials);
            // try and find the outline material
            Material outline = null;
            foreach (Material m in materials)
            {
                if(m.name.IndexOf("InkingMaterial") >= 0) outline = m;
            }
            // if we can't find it, load it
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
            _outlines.Add(_outline);
        }

        /*
        // for meshes on object children
        foreach(GameObject part in _parts)
        {
            SkinnedMeshRenderer skinnedMR;
            MeshRenderer meshRenderer = null;
            Mesh mesh = null;
            MeshFilter meshFilter;
            List<Material> materials = new();
            if(part.TryGetComponent(out skinnedMR))
            {
                mesh = skinnedMR.sharedMesh;
                materials = new(skinnedMR.materials);
            }else if(part.TryGetComponent(out meshFilter))
            {
                mesh = meshFilter.mesh;
                meshRenderer = part.GetComponent<MeshRenderer>();
                materials = new(meshRenderer.materials);
            }else
            {
                throw new Exception("No mesh renderer found");
            }
            mesh.subMeshCount+=1;
            mesh.SetTriangles(mesh.triangles,mesh.subMeshCount-1);
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
                    skinnedMR?.SetMaterials(materials);
                    meshRenderer?.SetMaterials(materials);
                }
            }
            _outlines.Add(outline);
        }*/
    }

    protected override void Start()
    {
        _canInteract = false;
        _timer = UnityEngine.Random.Range(_growMinTime,_growMaxTime);
        SFXManager.PlaySound(SFXManager.SoundType.CRASH,_crashVolume);
    }

    void Update()
    {
        if(Infested) return;
        _timer-=Time.deltaTime;
        if(_timer <= 0)
        {
            HullBreach hb = Instantiate(_infestedHullBreach,transform.position,transform.rotation);
            hb.transform.localScale*=1.5f;
            hb.Infested = true;
            Destroy(gameObject);
        }
    }

    public override void Interact(Player player)
    {
        // use the item

        EquippableItem item = player.PlayerInteract.EquippedItem;
        if(item != null && item.it == _correctItem)
        {
            SFXManager.PlaySound(SFXManager.SoundType.HAMMER_BONK,item.ClangVolume);
            BreachDestroyedOnce = true;
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
