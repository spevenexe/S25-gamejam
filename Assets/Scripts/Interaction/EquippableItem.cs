using UnityEngine;

public class EquippableItem : Item
{
    [SerializeField] public Vector3 targetRotation;

    public override void Interact(Player player)
    {
        // use the item
        if(player.PlayerInteract.EquippedItem == this)
        {
            UseOn(null,player);
        }
        // pick it up
        else if(player.PlayerInteract.EquippedItem == null)
        {
            player.PlayerInteract.Equip(this);
        }
    }

    protected override string UniqueToolTip()
    {
        return $"Equip {_itemName}";
    }

    // return true if there is a succesful use case
    internal virtual bool UseOn(Interactable interactable, Player player)
    {
        return false;
    }
}
