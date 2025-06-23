using UnityEngine;
using Items;
using System;

/// <summary>
/// A type of Interactable which needs a specific item to be held by the player to interact with correctly
/// </summary>
public abstract class InteractableNeedsItem : Interactable
{
    [SerializeField] protected ItemType _correctItem = ItemType.None;
    public ItemType CorrectItem { get => CorrectItem; }

    public override string MessageTooltip(EquippableItem equippedItem = null)
    {
        if ((equippedItem != null && _correctItem == equippedItem.it) || _correctItem == ItemType.None)
        {
            _canInteract = true;
            return base.MessageTooltip(equippedItem);
        }
        else
        {
            _canInteract = false;
            return $"Need [{_correctItem}]";
        }
    }
    

}
