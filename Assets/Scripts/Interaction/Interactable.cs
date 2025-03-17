using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public abstract class Interactable : MonoBehaviour
{
    protected static PlayerInput PInput; 
    public abstract void Interact(Player player);
    // return just the keys
    public virtual String MessageTooltip()
    {
        return Utils.getKeys(PInput,"Interact");
    }
    public static void setPI(PlayerInput pi)
    {
        PInput = pi;
    } 
}
