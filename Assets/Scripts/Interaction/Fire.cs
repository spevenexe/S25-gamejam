
public class Fire : Interactable
{
    //public float fireDamage = 10f; // damage per second
    public override void Interact(Player player)
    {
        // use the item
        if(player.PlayerInteract.EquippedItem != null)
        {
            EquippableItem item = player.PlayerInteract.EquippedItem as EquippableItem;
            if(item != null && item.UseOn(this, player))
            {
                Destroy(gameObject); // destroy the fire object after using the item
                // call the fixHullBreach method to fix the hull breach

            }

        }
    }

    protected override string UniqueToolTip(EquippableItem equippedItem)
    {
        throw new System.NotImplementedException();
    }

}
