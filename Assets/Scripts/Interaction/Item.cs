using UnityEngine;

public abstract class Item : Interactable
{
    public Rigidbody rb {get; private set;}
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
}
