using System;

/// <summary>
/// An Interactable that triggers an event. Useful for linking multiple functions to a particular interaction.
/// </summary>
public abstract class EventInteractable : Interactable, ITriggerEvent
{
    public event Action InteractionTriggers;

    public override void Interact(PData player)
    {
        if (_canInteract) InteractionTriggers.Invoke();
    }
}