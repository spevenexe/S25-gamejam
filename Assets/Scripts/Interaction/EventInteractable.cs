using System;
using UnityEngine;

public abstract class EventInteractable : Interactable
{
    protected event Action action;
    public override void Interact(Player player)
    {
        if (canInteract) action.Invoke();
    }
}
