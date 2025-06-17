/// <summary>
/// A type of item that cannot be held in a pocket, it must be carried around in front of the player, and prevents interaction with other objects while held.
/// </summary>
public class HeavyItem : Item
{
    public override void Interact(PData player)
    {
        EquippableItem heldItem = player.EquippedItem;
        // drop the heavy item we are holding
        if (this == player.HauledItem)
        {
            player.DropHeavyEvent.Invoke();
        }
        // if we are holding an item that is useable on this, try to use it
        else if (heldItem != null && heldItem.UseOn(this, player))
        {
        }
        // pick up the item if we can 
        else
        {
            player.HaulEvent.Invoke(this);
        }
    }

    protected override string UniqueToolTip(EquippableItem equippedItem)
    {
        return $"Pick Up {ItemName}";
    }
}
