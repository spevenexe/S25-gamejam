using System;
using System.Collections;
using UnityEngine;

public class Monitor : EventInteractable
{
    [SerializeField] private float _buttonCooldown = .5f;

    protected override void OnEnable()
    {
        base.OnEnable();
        InteractionTriggers+=PlaySound;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        InteractionTriggers-=PlaySound;
    }

    private void PlaySound() => SFXManager.PlaySound(SFXManager.SoundType.BUTTON);

    public override void Interact(Player player)
    {
        base.Interact(player);
        StartCoroutine(ButtonCooldown());
    }

    private IEnumerator ButtonCooldown()
    {
        _canInteract = false;
        yield return new WaitForSeconds(_buttonCooldown);
        _canInteract = true;
    }

    protected override string UniqueToolTip(EquippableItem equippedItem)
    {
        return "Realign Ship Navigation";
    } 
}
