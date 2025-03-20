using System;
using System.Collections;
using UnityEngine;

public class Monitor : EventInteractable
{
    [SerializeField] private float _buttonCooldown = .5f;

    protected override void OnEnable()
    {
        base.OnEnable();
        action+= () => SFXManager.PlaySound(SFXManager.SoundType.BUTTON);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        action-= () => SFXManager.PlaySound(SFXManager.SoundType.BUTTON);
    }

    public override void Interact(Player player)
    {
        base.Interact(player);
        StartCoroutine(ButtonCooldown());
    }

    private IEnumerator ButtonCooldown()
    {
        canInteract = false;
        yield return new WaitForSeconds(_buttonCooldown);
        canInteract = true;
    }

    public override string MessageTooltip()
    {
        string message = base.MessageTooltip();
        message+=" Navigate";
        return message;
    } 
}
