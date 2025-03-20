using System;
using UnityEngine;

public abstract class EventInteractable : Interactable
{
    public event Action InteractionTriggers;
    public override void Interact(Player player)
    {
        if (_canInteract) InteractionTriggers.Invoke();
    }
}
