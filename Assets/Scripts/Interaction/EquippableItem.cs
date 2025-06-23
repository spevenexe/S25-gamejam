using System.Collections;
using Items;
using UnityEngine;

/// <summary>
/// A type of item that can be held in the player's hand (also called pocket)
/// </summary>
public class EquippableItem : Item
{
    /// <summary>
    /// When the imported model is not normalized, this helps control how the item is oriented when dropped.
    /// </summary>
    public Vector3 targetRotation;

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

    /// <summary>
    /// Attemps to use the equipped item on the given <c>Interactable</c>.
    /// </summary>
    /// <param name="interactable">The interactable to use the item on.</param>
    /// <param name="player">the interacting player's data</param>
    /// <returns><c>true</c> if there is a succesful use case, <c> false</c> otherwise.</returns>
    internal virtual bool UseOn(Interactable interactable, PData player)
    {
        return false;
    }

    /// <summary>
    /// For running animations on the item.
    /// </summary>
    public virtual IEnumerator Animate()
    {
        yield break;
    }
}
