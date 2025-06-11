using System.Collections;
using UnityEngine;

public class EquippableItem : Item
{
    [SerializeField] public Vector3 targetRotation;

    public override void Interact(PInput player)
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

    protected override string UniqueToolTip(EquippableItem equippedItem)
    {
        return $"Equip {ItemName}";
    }

    // return true if there is a succesful use case
    internal virtual bool UseOn(Interactable interactable, PInput player)
    {
        return false;
    }

    // for running animations on the item
    public virtual IEnumerator Animate()
    {
        yield break;
    }
}
