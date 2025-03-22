using UnityEngine;

public abstract class InteractbleWithItem : EventInteractable
{
    public enum ItemType{
        None,
        Hammer,
        Tungsten_Cube,
        Cannon_Ball,
        Cargo,
        Dud_Grenade,
        Large_Explosive,
        Tungsten_Chunk
    }

    public override string MessageTooltip(EquippableItem equippedItem = null)
    {
        if((equippedItem != null && _correctItem == equippedItem.it) || _correctItem == ItemType.None)
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
