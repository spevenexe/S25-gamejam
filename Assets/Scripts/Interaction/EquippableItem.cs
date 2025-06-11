using System.Collections;
using UnityEngine;

public class EquippableItem : Item
{
    [SerializeField] public Vector3 targetRotation;

    public override void Interact(PData player)
    {
        // use the item
        if(player.EquippedItem == this)
        {
            UseOn(null,player);
        }
        // pick it up
        else if(player.EquippedItem == null)
        {
            player.EquipEvent.Invoke(this);
        }
    }

    protected override string UniqueToolTip(EquippableItem equippedItem)
    {
        return $"Equip {ItemName}";
    }

    // return true if there is a succesful use case
    internal virtual bool UseOn(Interactable interactable, PData player)
    {
        return false;
    }

    // for running animations on the item
    public virtual IEnumerator Animate()
    {
        yield break;
    }
}
