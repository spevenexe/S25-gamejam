using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class HeavyItem : Item
{
    public override void Interact(Player player)
    {
        EquippableItem heldItem = player.PlayerInteract.EquippedItem; // this may be useful?
        // drop the heavy item we are holding
        if(this == player.PlayerInteract.HauledItem)
        {
            player.PlayerInteract.DropHauledItem();
        }
        // we must have a held item
        else if (heldItem != null && heldItem.UseOn(this,player))
        {
        }
        // pick up the item if we can 
        else {
            player.PlayerInteract.Haul(this);
        }
    }

    public override string MessageTooltip()
    {
        string message = base.MessageTooltip();

        message+=" Pick Up "+name;
        return message;
    }
}
