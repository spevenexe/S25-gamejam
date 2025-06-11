public class HeavyItem : Item
{
    public override void Interact(PData player)
    {
        EquippableItem heldItem = player.EquippedItem; // this may be useful?
        // drop the heavy item we are holding
        if(this == player.HauledItem)
        {
            player.DropHeavyEvent.Invoke();
        }
        // we must have a held item
        else if (heldItem != null && heldItem.UseOn(this,player))
        {
        }
        // pick up the item if we can 
        else {
            player.HaulEvent.Invoke(this);
        }
    }

    protected override string UniqueToolTip(EquippableItem equippedItem)
    {
        return $"Pick Up {ItemName}";
    }
}
