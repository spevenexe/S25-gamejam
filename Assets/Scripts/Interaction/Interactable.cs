using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public abstract class Interactable : MonoBehaviour
{
    protected Material outline;
    protected static PlayerInput PInput; 
    public abstract void Interact(Player player);
    void Awake()
    {
        foreach (Material m in GetComponent<MeshRenderer>().materials)
        {
            if(m.name.IndexOf("InkingMaterial") >= 0) outline = m;
        }
    }
    // return just the keys
    public virtual String MessageTooltip()
    {
        return Utils.getKeys(PInput,"Interact");
    }
    public static void setPI(PlayerInput pi)
    {
        PInput = pi;
    } 

    public void highlight(Color color) => outline.SetColor("_Outline_Color",color);
}
