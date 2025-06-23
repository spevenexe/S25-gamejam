using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// A screen that can be clicked on.
/// </summary>
public class Monitor : EventInteractable
{
    [SerializeField] private float _buttonCooldown = .5f;
    [SerializeField] private string toolTipMessage = "Realign Ship Navigation";

    protected override void OnEnable()
    {
        base.OnEnable();
        InteractionTriggers += PlaySound;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        InteractionTriggers-=PlaySound;
    }

    private void PlaySound() => SFXManager.PlaySound(SFXManager.SoundType.BUTTON);

    public override void Interact(PData player)
    {
        base.Interact(player);
        StartCoroutine(ButtonCooldown());
    }

    /// <summary>
    /// buffers a few seconds to prevent the player from spamming the monitor
    /// </summary>
    private IEnumerator ButtonCooldown()
    {
        _canInteract = false;
        yield return new WaitForSeconds(_buttonCooldown);
        _canInteract = true;
    }

    protected override string UniqueToolTip(EquippableItem equippedItem)
    {
        return toolTipMessage;
    } 
}
