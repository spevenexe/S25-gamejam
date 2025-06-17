using System;
using UnityEngine;

public abstract class EventInteractable : Interactable
{
    
    [SerializeField] protected InteractableWithItem.ItemType _correctItem = InteractableWithItem.ItemType.None;
    public InteractableWithItem.ItemType CorrectItem {get => CorrectItem;}
    public event Action InteractionTriggers;

    public override void Interact(PData player)
    {
        if (_canInteract) InteractionTriggers.Invoke();
    }
}
