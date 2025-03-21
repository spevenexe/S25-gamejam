using System;
using UnityEngine;

public abstract class EventInteractable : Interactable
{
    
    [SerializeField] protected InteractbleWithItem.ItemType _correctItem = InteractbleWithItem.ItemType.None;
    public InteractbleWithItem.ItemType CorrectItem {get => CorrectItem;}
    public event Action InteractionTriggers;

    public override void Interact(Player player)
    {
        if (_canInteract) InteractionTriggers.Invoke();
    }
}
