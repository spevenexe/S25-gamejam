public class EquippableItem : Item
{
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

    // return true if there is a succesful use case
    internal virtual bool UseOn(Interactable interactable, Player player)
    {
        return false;
    }
}
