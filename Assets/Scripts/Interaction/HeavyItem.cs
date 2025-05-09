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

    protected override string UniqueToolTip(EquippableItem equippedItem)
    {
        return $"Pick Up {ItemName}";
    }
}
