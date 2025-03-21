using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Lever : EventInteractable{
    // string [] position = {"Up", "Middle", "Down"};
    public enum LeverState
    {
        up,
        down, 
        off,
    }
    [SerializeField] private LeverState _leverState = LeverState.off;
    public LeverState ActiveState {get {return _leverState;}}
    public LeverState CorrectState = LeverState.up;

    private LeverLight _leverLight;
    private List<Material> _outlines = new List<Material>();

    protected override void OnEnable()
    {
        base.OnEnable();
        InteractionTriggers+=Switch;
        InteractionTriggers+= () => SFXManager.PlaySound(SFXManager.SoundType.LEVER);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        InteractionTriggers-=Switch;
        InteractionTriggers-= () => SFXManager.PlaySound(SFXManager.SoundType.LEVER);
    }

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

        _leverLight = GetComponentInChildren<LeverLight>();
    }

    protected override void Start()
    {
        base.Start();

        Switch(_leverState);
    }

    private void Switch()
    {
        _leverState = (LeverState) (((int)_leverState + 1 )% 2) ;
        if(_leverState == CorrectState) _leverLight.ResetColorDefault();
        else _leverLight.SetAlarmColor();
    }
    
    private void Switch(LeverState newState)
    {
        _leverState = newState;
        if(_leverState == CorrectState) 
        {
            _leverLight.ResetColorDefault();
            _canInteract = true;
        }
        else if(_leverState == LeverState.off) 
        {
            _leverLight.TurnOff();
            _canInteract = false;
        }
        else 
        {
            _leverLight.SetAlarmColor();
            _canInteract = true;
        }
    }

    public void Activate()
    {
        Switch(LeverState.up);
    }
    public void Deactivate()
    {
        Switch(LeverState.off);
    }

    public override void highlight(Color color)
    {
        foreach(Material outline in _outlines)
        {
            if (_canInteract) outline?.SetColor("_Outline_Color",color);
            else outline?.SetColor("_Outline_Color",Color.black);
        }
    }

    protected override String UniqueToolTip(EquippableItem equippedItem)
    {
        return $"Flick Lever";
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Lever))]
public class LeverInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Lever lever = (Lever) target;
        if(GUILayout.Button("Activate",GUILayout.Width(90f)))
        {
            lever.Activate();
        }
        
        if(GUILayout.Button("Deactivate",GUILayout.Width(90f)))
        {
            lever.Deactivate();
        }
    }
}
#endif