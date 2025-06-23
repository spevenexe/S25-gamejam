
public class Fire : Interactable
{
    //public float fireDamage = 10f; // damage per second
    public override void Interact(PData player)
    {
        // use the item
        if(player.EquippedItem != null)
        {
            EquippableItem item = player.EquippedItem as EquippableItem;
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
