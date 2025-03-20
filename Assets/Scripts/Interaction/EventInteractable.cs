using System;
using UnityEngine;

public abstract class EventInteractable : Interactable
{
    protected event Action action;
    public override void Interact(Player player)
    {
        if (_canInteract) action.Invoke();
    }

    public virtual void AddEvent(Action a) => action+=a;

    public virtual void RemoveEvent(Action a) => action-=a;
}
