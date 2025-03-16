using UnityEditor.Callbacks;
using UnityEngine;

public class Item : Interactable
{
    private Rigidbody _rb;
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    // empty. we handle item pick ups with the player script
    public override void Interact(Player player)
    {
        Debug.Log(gameObject+" interacted.");
        if(this == player.PlayerInteract.HeldItem){
            player.PlayerInteract.DropItem();
            _rb.useGravity = true;
            _rb.excludeLayers = LayerMask.GetMask("Nothing"); 
        }
        else{
            _rb.useGravity = false;
            _rb.excludeLayers = LayerMask.GetMask("Player"); 
            _rb.linearVelocity = Vector3.zero;
            player.PlayerInteract.PickUp(this);
        }
    }
}
