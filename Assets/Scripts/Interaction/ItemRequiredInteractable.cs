using UnityEngine;

public abstract class InteractbleWithItem : EventInteractable
{
    public enum ItemType{
        None,
        Hammer,
        Cube
    }

    public override string MessageTooltip(EquippableItem equippedItem = null)
    {
        if((equippedItem != null && _correctItem == equippedItem.ItemName) || _correctItem == InteractbleWithItem.ItemType.None)
        {
            _canInteract = true;
            return base.MessageTooltip(equippedItem);
        }
        else{
            _canInteract = false;
            return "";
        }
    }
}
